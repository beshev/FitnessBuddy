namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Foods;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FoodsController : Controller
    {
        private const int FoodsPerPage = 6;

        private readonly IFoodsService foodsService;
        private readonly IUsersService usersService;

        public FoodsController(
            IFoodsService foodsService,
            IUsersService usersService)
        {
            this.foodsService = foodsService;
            this.usersService = usersService;
        }

        [AllowAnonymous]
        public IActionResult All(int id = 1, string search = null)
        {
            if (id < 1)
            {
                id = 1;
            }

            int count = this.foodsService.GetCount(null, search);
            int pagesCount = (int)Math.Ceiling((double)count / FoodsPerPage);

            if (pagesCount > 0 && id > pagesCount)
            {
                id = pagesCount;
            }

            int skip = (id - 1) * FoodsPerPage;
            var foods = this.foodsService.GetAll<FoodViewModel>(null, search, skip, FoodsPerPage);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Foods = foods,
                Search = search,
                ForAction = nameof(this.All),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public IActionResult MyFoods(int id = 1, string search = null)
        {
            if (id < 1)
            {
                id = 1;
            }

            var userId = this.User.GetUserId();
            int count = this.foodsService.GetCount(userId, search);
            int pagesCount = (int)Math.Ceiling((double)count / FoodsPerPage);

            if (pagesCount > 0 && id > pagesCount)
            {
                id = pagesCount;
            }

            int skip = (id - 1) * FoodsPerPage;
            var foods = this.foodsService.GetAll<FoodViewModel>(userId, search, skip, FoodsPerPage);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Foods = foods,
                Search = search,
                ForAction = nameof(this.MyFoods),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public IActionResult Favorites(int id = 1)
        {
            if (id < 1)
            {
                id = 1;
            }

            var userId = this.User.GetUserId();
            int count = this.usersService.FavoriteFoodsCount(userId);
            int pagesCount = (int)Math.Ceiling((double)count / FoodsPerPage);

            if (pagesCount > 0 && id > pagesCount)
            {
                id = pagesCount;
            }

            int skip = (id - 1) * FoodsPerPage;
            var foods = this.usersService.GetFavoriteFoods(userId, skip, FoodsPerPage);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Foods = foods,
                ForAction = nameof(this.Favorites),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(FoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.foodsService.AddAsync(userId, model);

            this.TempData[GlobalConstants.NameOfSuccess] = string.Format(GlobalConstants.SuccessMessage, GlobalConstants.NameOfFood);

            return this.RedirectToAction(nameof(this.MyFoods));
        }

        public async Task<IActionResult> AddToFavorite(int pageNumber, int foodId)
        {
            if (this.foodsService.IsExist(foodId) == false)
            {
                return this.NotFound();
            }

            var food = this.foodsService.GetById(foodId);
            var userId = this.User.GetUserId();

            await this.usersService.AddFoodToFavoriteAsync(userId, food);

            this.TempData[GlobalConstants.NameOfSuccess] = GlobalConstants.AddFoodToFavoriteMessage;

            return this.RedirectToAction(nameof(this.All), new { Id = pageNumber });
        }

        public async Task<IActionResult> RemoveFromFavorite(int pageNumber, int foodId)
        {
            if (this.foodsService.IsExist(foodId) == false)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();
            var food = this.foodsService.GetById(foodId);

            await this.usersService.RemoveFoodFromFavoriteAsync(userId, food);

            this.TempData[GlobalConstants.NameOfDelete] = GlobalConstants.RemoveFoodFromFavoriteMessage;

            return this.RedirectToAction(nameof(this.Favorites), new { Id = pageNumber });
        }

        public IActionResult Edit(int id)
        {
            if (this.foodsService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            if (this.foodsService.IsUserFood(this.User.GetUserId(), id) == false)
            {
                return this.Unauthorized();
            }

            var food = this.foodsService.GetById(id);

            var viewModel = AutoMapperConfig.MapperInstance.Map<Food, FoodInputModel>(food);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodInputModel model)
        {
            if (this.foodsService.IsExist(model.Id) == false)
            {
                return this.NotFound();
            }

            if (this.foodsService.IsUserFood(this.User.GetUserId(), model.Id) == false)
            {
                return this.Unauthorized();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.foodsService.EditAsync(model);

            this.TempData[GlobalConstants.NameOfSuccess] = GlobalConstants.EditFoodMessage;

            return this.RedirectToAction(nameof(this.MyFoods));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.foodsService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            if (this.foodsService.IsUserFood(this.User.GetUserId(), id) == false)
            {
                return this.Unauthorized();
            }

            await this.foodsService.DeleteAsync(id);

            this.TempData[GlobalConstants.NameOfDelete] = string.Format(GlobalConstants.DeleteMessage, GlobalConstants.NameOfFood);

            return this.RedirectToAction(nameof(this.MyFoods));
        }
    }
}
