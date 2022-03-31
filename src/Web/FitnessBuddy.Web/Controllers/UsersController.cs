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
            var userId = await this.userService.GetIdByUsernameAsync(username);

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = loggedUserId;
            }

            var viewModel = await this.userService.GetProfileDataAsync(userId);

            if (tab == GlobalConstants.NameOfFollowers)
            {
                viewModel.Followers = await this.userService.GetFollowersAsync<UserFollowers>(userId);
            }

            if (tab == GlobalConstants.NameOfFollowing)
            {
                viewModel.Following = await this.userService.GetFollowingAsync<UserFollowing>(userId);
            }

            viewModel.IsMyProfile = userId == loggedUserId;
            viewModel.IsFollowingByUser = await this.usersFollowersService.IsFollowingByUserAsync(userId, this.User.GetUserId());

            return this.View(viewModel);
        }

        public async Task<IActionResult> Follow(string username)
        {
            var userId = await this.userService.GetIdByUsernameAsync(username);
            var followerId = this.User.GetUserId();

            if (userId == null)
            {
                return this.NotFound();
            }

            if (await this.usersFollowersService.IsFollowingByUserAsync(userId, followerId))
            {
                return this.Unauthorized();
            }

            await this.usersFollowersService.FollowAsync(userId, followerId);

            return this.RedirectToAction(nameof(this.Profile), new { Username = username });
        }

        public async Task<IActionResult> UnFollow(string username)
        {
            var userId = await this.userService.GetIdByUsernameAsync(username);
            var followerId = this.User.GetUserId();

            if (userId == null)
            {
                return this.NotFound();
            }

            if (await this.usersFollowersService.IsFollowingByUserAsync(userId, followerId) == false)
            {
                return this.Unauthorized();
            }

            await this.usersFollowersService.UnFollowAsync(userId, followerId);

            return this.RedirectToAction(nameof(this.Profile), new { Username = username });
        }

        public async Task<IActionResult> All(int id = 1, string username = "")
        {
            this.ViewData["Username"] = username;

            if (id < 1)
            {
                return this.NotFound();
            }

            var usersPerPage = 6;
            int count = await this.userService.GetCountAsync();
            int pagesCount = (int)Math.Ceiling((double)count / usersPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * usersPerPage;

            var users = await this.userService.GetAllAsync<ShortUserViewModel>(username, skip, usersPerPage);

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

        public async Task<IActionResult> Edit()
        {
            var userId = this.User.GetUserId();

            var viewModel = await this.userService.GetUserInfoAsync<UserInputModel>(userId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserInputModel model)
        {
            if (await this.userService.IsUsernameExistAsync(model.Username))
            {
                this.ModelState.AddModelError("Username", "Username is taken");
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.userService.EditAsync(userId, model);

            return this.RedirectToAction(nameof(this.Profile));
        }
    }
}
