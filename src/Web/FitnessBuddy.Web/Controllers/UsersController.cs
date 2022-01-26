namespace FitnessBuddy.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private readonly IUsersService userService;
        private readonly IMealsService mealsService;

        public UsersController(
            IUsersService userService,
            IMealsService mealsService)
        {
            this.userService = userService;
            this.mealsService = mealsService;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = this.User.GetUserId();
            var userInfo = this.userService.GetUserInfo<UserViewModel>(userId);
            var userMeals = this.mealsService.GetUserMeals<MealViewModel>(userId);

            var viewModel = AutoMapperConfig.MapperInstance.Map<IEnumerable<MealViewModel>, ProfileViewModel>(userMeals);
            viewModel.UserInfo = userInfo;
            viewModel.UserEmail = this.User.GetUserEmail();

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Edit()
        {
            var userId = this.User.GetUserId();

            var userData = this.userService.GetUserInfo<UserViewModel>(userId);

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
