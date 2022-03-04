namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult All(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var usersPerPage = 15;
            int count = this.usersService.GetCount();
            int pagesCount = (int)Math.Ceiling((double)count / usersPerPage);

            if (id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * usersPerPage;

            var users = this.usersService.GetAll<AdministrationUserViewModel>(null, skip, usersPerPage);

            var viewModel = new AllUsersViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Users = users,
                ForAction = nameof(this.All),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public IActionResult Ban(string username)
        {
            var viewModel = new UserBanInputModel
            {
                Username = username,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(UserBanInputModel model)
        {
            await this.usersService.BanUserAsync(model.Username, model.BanReason);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Unban(string username)
        {
            await this.usersService.UnbanUserAsync(username);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
