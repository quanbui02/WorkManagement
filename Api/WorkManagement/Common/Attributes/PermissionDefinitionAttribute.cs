using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace WorkManagement.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class PermissionDefinitionAttribute : ActionFilterAttribute
    {
        private string _permissionName { get; set; }

        private int? _permissionIndex { get; set; }

        public string PermissionName => _permissionName;

        public int? PermissionIndex => _permissionIndex;

        public bool AllowAllAuthentcatedUser { get; set; }

        public string GroupName { get; set; }

        public PermissionDefinitionAttribute(string permissionName)
        {
            _permissionName = permissionName;
        }

        public PermissionDefinitionAttribute(string permissionName, int permissionIndex)
        {
            _permissionName = permissionName;
            _permissionIndex = permissionIndex;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() == null)
                {
                    if (!context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    if (!AllowAllAuthentcatedUser && _permissionIndex.HasValue)
                    {
                        Claim claim = context.HttpContext.User.FindFirst("issuperuser");
                        if (claim == null)
                        {
                            goto IL_00e2;
                        }

                        bool result = false;
                        bool.TryParse(claim.Value, out result);
                        if (!result)
                        {
                            goto IL_00e2;
                        }
                    }
                }

                goto end_IL_0001;
            IL_03d4:
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new ForbidResult();
                goto end_IL_0001;
            IL_00e2:
                IConfiguration configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
                Claim claim2 = context.HttpContext.User.FindFirst("permissions");
                if (claim2 != null)
                {
                    string currentServiceName2 = "";
                    string value = configuration.GetValue("spring:application:rawname", "");
                    if (string.IsNullOrEmpty(value))
                    {
                        currentServiceName2 = configuration.GetValue("spring:application:name", "");
                    }
                    else
                    {
                        currentServiceName2 = value;
                    }

                    Dictionary<string, Dictionary<string, long>> source = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, long>>>(claim2.Value);
                    KeyValuePair<string, Dictionary<string, long>> keyValuePair = source.SingleOrDefault((KeyValuePair<string, Dictionary<string, long>> p) => p.Key.Equals(currentServiceName2, StringComparison.OrdinalIgnoreCase));
                    if (keyValuePair.Equals(default(KeyValuePair<string, Dictionary<string, long>>)))
                    {
                        goto IL_03d4;
                    }

                    string currentController2 = (context.Controller as Controller).GetType().Name.Replace("Controller", "");
                    KeyValuePair<string, long> keyValuePair2 = keyValuePair.Value.SingleOrDefault((KeyValuePair<string, long> c) => c.Key.Equals(currentController2, StringComparison.OrdinalIgnoreCase));
                    if (keyValuePair2.Equals(default(KeyValuePair<string, long>)))
                    {
                        goto IL_03d4;
                    }

                    long value2 = keyValuePair2.Value;
                    int num = 1 << _permissionIndex.Value;
                    if ((value2 & num) != num)
                    {
                        goto IL_03d4;
                    }
                }
                else
                {
                    StringValues stringValues = context.HttpContext.Request.Headers["permissions"];
                    string value3 = configuration.GetValue("spring:application:rawname", "");
                    string currentServiceName = "";
                    if (string.IsNullOrEmpty(value3))
                    {
                        currentServiceName = configuration.GetValue("spring:application:name", "");
                    }
                    else
                    {
                        currentServiceName = value3;
                    }

                    Dictionary<string, Dictionary<string, long>> source2 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, long>>>(Base64Decode((string?)stringValues));
                    KeyValuePair<string, Dictionary<string, long>> keyValuePair3 = source2.SingleOrDefault((KeyValuePair<string, Dictionary<string, long>> p) => p.Key.Equals(currentServiceName, StringComparison.OrdinalIgnoreCase));
                    if (keyValuePair3.Equals(default(KeyValuePair<string, Dictionary<string, long>>)))
                    {
                        goto IL_03d4;
                    }

                    string currentController = (context.Controller as Controller).GetType().Name.Replace("Controller", "");
                    KeyValuePair<string, long> keyValuePair4 = keyValuePair3.Value.SingleOrDefault((KeyValuePair<string, long> c) => c.Key.Equals(currentController, StringComparison.OrdinalIgnoreCase));
                    if (keyValuePair4.Equals(default(KeyValuePair<string, long>)))
                    {
                        goto IL_03d4;
                    }

                    long value4 = keyValuePair4.Value;
                    int num2 = 1 << _permissionIndex.Value;
                    if ((value4 & num2) != num2)
                    {
                        goto IL_03d4;
                    }
                }

            end_IL_0001:;
            }
            catch (Exception)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new StatusCodeResult(500);
                return;
            }

            base.OnActionExecuting(context);
        }

        public string Base64Decode(string base64EncodedData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
