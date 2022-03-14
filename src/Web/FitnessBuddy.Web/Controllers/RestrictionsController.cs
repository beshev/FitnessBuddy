namespace FitnessBuddy.Web.Controllers
{
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Restrictions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RestrictionsController : Controller
    {
        private readonly IUsersService usersService;

        public RestrictionsController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Ban()
        {
            var userId = this.User.GetUserId();

            if (this.usersService.IsUserBanned(userId) == false)
            {
                return this.NotFound();
            }

            var viewModel = this.usersService.GetUserInfo<UserBanViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
