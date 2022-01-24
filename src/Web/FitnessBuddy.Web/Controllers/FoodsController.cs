namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Foods;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class FoodsController : Controller
    {
        private readonly IFoodsService foodsService;
        private readonly IUsersService usersService;

        public FoodsController(
            IFoodsService foodsService,
            IUsersService usersService)
        {
            this.foodsService = foodsService;
            this.usersService = usersService;
        }

        public IActionResult All()
        {
            var viewModel = this.foodsService.GetAll();

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult MyFoods()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.foodsService.FoodsAddedByUser(userId);

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(FoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.foodsService.AddAsync(userId, model);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> AddToFavorite(int foodId)
        {
            var food = this.foodsService.GetFoodById(foodId);
            var userId = this.User.GetUserId();

            await this.usersService.AddFoodToFavoriteAsync(userId, food);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromFavorite(int foodId)
        {
            var userId = this.User.GetUserId();
            var food = this.foodsService.GetFoodById(foodId);

            await this.usersService.RemoveFoodFromFavoriteAsync(userId, food);

            return this.RedirectToAction(nameof(this.Favorites));
        }

        [Authorize]
        public IActionResult Favorites()
        {
            var userId = this.User.GetUserId();

            var viewModel = this.usersService.GetFavoriteFoods(userId);

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var food = this.foodsService.GetFoodById(id);

            if (food.AddedByUserId != this.User.GetUserId())
            {
                return this.RedirectToAction(nameof(this.MyFoods));
            }

            var viewModel = AutoMapperConfig.MapperInstance.Map<Food, FoodInputModel>(food);

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(FoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.foodsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await this.foodsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.MyFoods));
        }
    }
}
