namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

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

        public async Task<IActionResult> Ban()
        {
            var userId = this.User.GetUserId();

            if (await this.usersService.IsUserBannedAsync(userId) == false)
            {
                return this.NotFound();
            }

            var viewModel = await this.usersService.GetUserInfoAsync<UserBanViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
