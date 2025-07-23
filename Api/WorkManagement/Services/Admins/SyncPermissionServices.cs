using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Work.DataContext;
using Work.DataContext.Identity;
using WorkManagement.Common;

namespace WorkManagement.Services.Admins
{

    public interface ISyncPermissionServices
    {
        Task<object> ScanAndSavePermissionsAsync();
    }
    public class SyncPermissionServices : ISyncPermissionServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInfo _userInfo;
        private readonly AppDbContext _db;
        public SyncPermissionServices(AppDbContext db, IConfiguration configuration, IUserInfo userInfo)
        {
            _db = db;
            _configuration = configuration;
            _userInfo = userInfo;
        }

        public async Task<object> ScanAndSavePermissionsAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes()
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);
            int indexController = 0;
            foreach (var controller in controllers)
            {
                string controllerName = controller.Name.Replace("Controller", "");
                string route = controller.GetCustomAttribute<RouteAttribute>()?.Template ?? controllerName;
                var controllerAttr = controller.GetCustomAttribute<PermissionDefinitionAttribute>();

                var appController = _db.AppController.FirstOrDefault(c => c.Name == controllerName);
                if (appController == null)
                {
                    appController = new AppController
                    {
                        Name = controllerName,
                        DisplayName = controllerAttr?.PermissionName ?? controllerName,
                        Route = route,
                        GroupName = controllerAttr?.GroupName ?? "",
                        Index = controllerAttr?.PermissionIndex ?? indexController++
                    };
                    _db.AppController.Add(appController);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    appController.DisplayName = controllerAttr?.PermissionName ?? controllerName;
                    appController.Route = route;
                    appController.GroupName = controllerAttr?.GroupName ?? "";
                    appController.Index = controllerAttr?.PermissionIndex ?? indexController++;
                    _db.AppController.Update(appController);
                    await _db.SaveChangesAsync();
                }

                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    var permissionAttr = method.GetCustomAttribute<PermissionDefinitionAttribute>();
                    if (permissionAttr == null)
                        continue;

                    var httpAttr = method.GetCustomAttributes<HttpMethodAttribute>().FirstOrDefault();
                    string methodRoute = httpAttr?.Template ?? method.Name;
                    string httpMethod = httpAttr?.HttpMethods?.FirstOrDefault() ?? "GET";

                    string permissionCode = $"{controllerName}.{method.Name}";

                    var existingPermission = _db.AppPermission.FirstOrDefault(p => p.PermissionCode == permissionCode);

                    if (existingPermission == null)
                    {
                        var permission = new AppPermission
                        {
                            Name = method.Name,
                            DisplayName = permissionAttr.PermissionName,
                            Controller = controllerName,
                            Route = methodRoute,
                            PermissionCode = permissionCode,
                            Index = permissionAttr.PermissionIndex ?? 0,
                            Description = permissionAttr.PermissionName,
                            AppControllerId = appController.Id
                        };
                        _db.AppPermission.Add(permission);
                    }
                    else
                    {
                        existingPermission.Name = method.Name;
                        existingPermission.DisplayName = permissionAttr.PermissionName;
                        existingPermission.Route = methodRoute;
                        existingPermission.Index = permissionAttr.PermissionIndex ?? 0;
                        existingPermission.Description = permissionAttr.PermissionName;
                        existingPermission.AppControllerId = appController.Id;
                        _db.AppPermission.Update(existingPermission);
                    }
                }
            }

            await _db.SaveChangesAsync();

            return Result<object>.Success(null, 1, "Đồng bộ Permission thành công.");
        }
    }
}
