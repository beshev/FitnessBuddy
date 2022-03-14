namespace FitnessBuddy.Web.Infrastructure.Filters
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class RestrictBannedUsersAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RestrictBannedUsersAttribute(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(context.HttpContext.User);

                if (user.IsBanned)
                {
                    context.Result = new RedirectResult(GlobalConstants.RestrictionBan);
                }
            }
        }
    }
}
