using System;
using System.Linq;
using System.Threading.Tasks;
using fcu_ucan.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace fcu_ucan.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;
            
            var userId = context.HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userId == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            
            var securityStamp = context.HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid")?.Value;
            if (securityStamp == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            try
            {
                var userManager = (UserManager<ApplicationUser>)context.HttpContext.RequestServices
                    .GetService(typeof(UserManager<ApplicationUser>)) ?? throw new ArgumentNullException();
                
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                    return;
                }

                if (securityStamp != user.SecurityStamp)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }
                if (!user.IsEnable)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }
            }
            catch (ArgumentNullException)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}