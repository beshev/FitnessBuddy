namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private readonly IUsersService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IUsersService userService,
            UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = this.User.GetUserId();
            var userInfo = this.userService.GetUserInfo(userId);

            // TODO: Add user current nutritions
            var viewModel = new ProfileViewModel
            {
                UserEmail = this.User.GetUserEmail(),
                CurrentProtein = 0,
                CurrentCarbohydrates = 0,
                CurrentFat = 0,
                UserInfo = userInfo,
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Edit()
        {
            var userId = this.User.GetUserId();

            var userData = this.userService.GetUserInfo(userId);

            var viewModel = AutoMapperConfig.MapperInstance
                .Map<UserViewModel, UserInputModel>(userData);

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserInputModel viewModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View();
            }

            var userId = this.User.GetUserId();

            await this.userService.EditAsync(userId, viewModel);

            return this.RedirectToAction(nameof(this.Profile));
        }
    }
}
