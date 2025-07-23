using WorkManagement.Common;
using WorkManagement.Extensions;
using WorkManagement.Helper;
using WorkManagement.Services.Admins;
namespace WorkManagement
{
    public static class StartUpExtensions
    {
        public static void AddCustomService(this IServiceCollection services, IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("DefaultConnection");
            services.DependencyInjectionDatacontext(configuration, connStr);
            #region Dependency Injection
            services.AddScoped<IAuthService, AuthServices>();
            services.AddScoped<IAppRolesServices, AppRolesServices>();
            services.AddScoped<IUserInfo, UserInfo>();
            services.AddScoped<ISyncPermissionServices, SyncPermissionServices>();

            #endregion Dependency Injection
        }

        public static void AddDebugCustomService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDiscoveryClient, FakeDiscoveryClient>();
        }
    }
}
