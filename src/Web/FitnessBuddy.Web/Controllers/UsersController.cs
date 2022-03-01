namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IUsersService userService;
        private readonly IMealsService mealsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(
            RoleManager<ApplicationRole> roleManager,
            IUsersService userService,
            IMealsService mealsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.roleManager = roleManager;
            this.userService = userService;
            this.mealsService = mealsService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> MyProfile()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.userService.GetProfileData(userId);

            viewModel.IsMyProfile = true;
            viewModel.UserInfo.UserRole = (await this.roleManager.FindByIdAsync(viewModel.UserInfo.UserRoleId)).Name;

            return this.View(viewModel);
        }

        public async Task<IActionResult> UserProfile(string username = "")
        {
            var userId = this.userService.GetIdByUsername(username);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.NotFound();
            }

            var viewModel = this.userService.GetProfileData(userId);

            viewModel.IsMyProfile = false;
            viewModel.UserInfo.UserRole = (await this.roleManager.FindByIdAsync(viewModel.UserInfo.UserRoleId)).Name;
            viewModel.IsFollowingByUser = this.userService.IsFollowingByUser(username, this.User.GetUsername());

            return this.View(viewModel);
        }

        public IActionResult Followers()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.userService.GetFollowers<UserFollowers>(userId);

            return this.View(viewModel);
        }

        public IActionResult Following()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.userService.GetFollowing<UserFollowing>(userId);

            return this.View(viewModel);
        }

        public IActionResult All(string username = "")
        {
            var viewModel = this.userService.GetAll<ShortUserViewModel>(username);

            this.ViewData["Username"] = username;

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
