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

        public IActionResult All(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            int itemsPerPage = 8;
            int pageNumber = id - 1;

            var foods = this.foodsService.GetAll<FoodViewModel>(null, pageNumber, itemsPerPage);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                ItemsPerPage = itemsPerPage,
                ItemsCount = this.foodsService.GetCount(),
                Foods = foods,
                ForAction = nameof(this.All),
            };

            if (viewModel.PagesCount < id)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult MyFoods(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();
            const int itemsPerPage = 8;
            var foods = this.foodsService.GetAll<FoodViewModel>(userId, id - 1, itemsPerPage);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                ItemsPerPage = itemsPerPage,
                ItemsCount = this.foodsService.GetCount(userId),
                Foods = foods,
                ForAction = nameof(this.MyFoods),
            };

            if (viewModel.PagesCount < id)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Favorites(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();
            const int itemsPerPage = 8;
            var foods = this.usersService.GetFavoriteFoods(userId);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                ItemsPerPage = itemsPerPage,
                ItemsCount = this.foodsService.GetCount(userId),
                Foods = foods,
                ForAction = nameof(this.Favorites),
            };

            if (viewModel.PagesCount < id)
            {
                return this.NotFound();
            }

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
        public async Task<IActionResult> AddToFavorite(int? foodId)
        {
            var food = this.foodsService.GetById(foodId.Value);
            var userId = this.User.GetUserId();

            await this.usersService.AddFoodToFavoriteAsync(userId, food);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromFavorite(int foodId)
        {
            var userId = this.User.GetUserId();
            var food = this.foodsService.GetById(foodId);

            await this.usersService.RemoveFoodFromFavoriteAsync(userId, food);

            return this.RedirectToAction(nameof(this.Favorites));
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(this.MyFoods));
            }

            var food = this.foodsService.GetById(id.Value);

            if (food == null || food.AddedByUserId != this.User.GetUserId())
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
        public async Task<IActionResult> Delete(int? foodId)
        {
            var userId = this.User.GetUserId();

            if (foodId != null && this.foodsService.IsUserFood(userId, foodId.Value))
            {
                await this.foodsService.DeleteAsync(foodId.Value);
            }

            return this.RedirectToAction(nameof(this.MyFoods));
        }
    }
}
