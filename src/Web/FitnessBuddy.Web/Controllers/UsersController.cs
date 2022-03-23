namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Data.UsersFollowers;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IUsersService userService;
        private readonly IUsersFollowersService usersFollowersService;
        private readonly IMealsService mealsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(
            RoleManager<ApplicationRole> roleManager,
            IUsersService userService,
            IUsersFollowersService usersFollowersService,
            IMealsService mealsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.roleManager = roleManager;
            this.userService = userService;
            this.usersFollowersService = usersFollowersService;
            this.mealsService = mealsService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Profile(string username = "", string tab = "")
        {
            this.ViewData[GlobalConstants.NameOfTab] = tab.ToLower();

            var loggedUserId = this.User.GetUserId();
            var userId = this.userService.GetIdByUsername(username);

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = loggedUserId;
            }

            var viewModel = this.userService.GetProfileData(userId);

            if (tab == GlobalConstants.NameOfFollowers)
            {
                viewModel.Followers = this.userService.GetFollowers<UserFollowers>(userId);
            }

            if (tab == GlobalConstants.NameOfFollowing)
            {
                viewModel.Following = this.userService.GetFollowing<UserFollowing>(userId);
            }

            viewModel.IsMyProfile = userId == loggedUserId;
            viewModel.UserInfo.UserRole = (await this.roleManager.FindByIdAsync(viewModel.UserInfo.UserRoleId)).Name;
            viewModel.IsFollowingByUser = this.usersFollowersService.IsFollowingByUser(userId, this.User.GetUserId());

            return this.View(viewModel);
        }

        public async Task<IActionResult> Follow(string username)
        {
            var userId = this.userService.GetIdByUsername(username);
            var followerId = this.User.GetUserId();

            if (userId == null)
            {
                return this.NotFound();
            }

            if (this.usersFollowersService.IsFollowingByUser(userId, followerId))
            {
                return this.Unauthorized();
            }

            await this.usersFollowersService.FollowAsync(userId, followerId);

            return this.RedirectToAction(nameof(this.Profile), new { Username = username });
        }

        public async Task<IActionResult> UnFollow(string username)
        {
            var userId = this.userService.GetIdByUsername(username);
            var followerId = this.User.GetUserId();

            if (userId == null)
            {
                return this.NotFound();
            }

            if (this.usersFollowersService.IsFollowingByUser(userId, followerId) == false)
            {
                return this.Unauthorized();
            }

            await this.usersFollowersService.UnFollowAsync(userId, followerId);

            return this.RedirectToAction(nameof(this.Profile), new { Username = username });
        }

        public IActionResult All(int id = 1, string username = "")
        {
            this.ViewData["Username"] = username;

            if (id < 1)
            {
                return this.NotFound();
            }

            var usersPerPage = 6;
            int count = this.userService.GetCount();
            int pagesCount = (int)Math.Ceiling((double)count / usersPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * usersPerPage;

            var users = this.userService.GetAll<ShortUserViewModel>(username, skip, usersPerPage);

            var viewModel = new UserListViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Users = users,
                ForAction = nameof(this.All),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

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

            return this.RedirectToAction(nameof(this.Profile));
        }
    }
}
