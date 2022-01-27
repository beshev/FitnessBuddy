namespace FitnessBuddy.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private readonly IUsersService userService;
        private readonly IMealsService mealsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(
            IUsersService userService,
            IMealsService mealsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.mealsService = mealsService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = this.User.GetUserId();
            var userInfo = this.userService.GetUserInfo<UserViewModel>(userId);
            var userMeals = this.mealsService.GetUserMeals<MealViewModel>(userId);

            var extention = Path.GetExtension(userInfo.ProfilePicture);
            userInfo.ProfilePicture = $"/images/profileimages/{userId}{extention}";

            var viewModel = AutoMapperConfig.MapperInstance.Map<IEnumerable<MealViewModel>, ProfileViewModel>(userMeals);
            viewModel.UserInfo = userInfo;
            viewModel.UserEmail = this.User.GetUserEmail();

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Edit()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.userService.GetUserInfo<UserInputModel>(userId);

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();
            var path = $"{this.webHostEnvironment.WebRootPath}/images";

            await this.userService.EditAsync(userId, model, path);

            return this.RedirectToAction(nameof(this.Profile));
        }
    }
}
