
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;

namespace SchoolApplication.Expire
{
    public class CustomAuthenticationFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public CustomAuthenticationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
           
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                // Check the user's role
                if (user.IsInRole("Student"))
                {
                    // Check the expiration time span
                    var expirationTime = user.FindFirst("ExpirationTime")?.Value;
                    if (!string.IsNullOrEmpty(expirationTime) && DateTime.TryParse(expirationTime, out DateTime expirationDateTime))
                    {
                        if (DateTime.Now > expirationDateTime)
                        {
                            // Redirect to the login page with a custom error message
                            var loginPath = "/Login/StudentLogin"; // Change to your login path
                            var returnUrl = _httpContextAccessor.HttpContext.Request.Path;
                            context.Result = new RedirectResult($"{loginPath}?returnUrl={returnUrl}&error=expired");
                        }
                    }
                }
            }
        }

        
    }
}
