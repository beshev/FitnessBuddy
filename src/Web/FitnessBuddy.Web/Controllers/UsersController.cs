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

    [Authorize]
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

        public IActionResult MyProfile()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.userService.GetProfileData(userId);
            viewModel.IsMyProfile = true;

            return this.View(viewModel);
        }

        public IActionResult UserProfile(string username = "")
        {
            var userId = this.userService.GetIdByUsername(username);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.NotFound();
            }

            var viewModel = this.userService.GetProfileData(userId);

            viewModel.IsMyProfile = false;

            return this.View(viewModel);
        }

        public IActionResult All()
        {
            var viewModel = this.userService.GetAll<ShortUserViewModel>();

            return this.View(viewModel);
        }

        public IActionResult Edit()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.userService.GetUserInfo<UserInputModel>(userId);

            return this.View(viewModel);
        }

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

            return this.RedirectToAction(nameof(this.MyProfile));
        }
    }
}
