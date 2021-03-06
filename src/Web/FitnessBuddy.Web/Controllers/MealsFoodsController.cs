namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.MealsFoodsService;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Meals;
    using Microsoft.AspNetCore.Mvc;

    public class MealsFoodsController : BaseController
    {
        private readonly IFoodsService foodsService;
        private readonly IMealsService mealsService;
        private readonly IMealsFoodsService mealsFoodsService;
        private readonly IUsersService usersService;

        public MealsFoodsController(
            IFoodsService foodsService,
            IMealsService mealsService,
            IMealsFoodsService mealsFoodsService,
            IUsersService usersService)
        {
            this.foodsService = foodsService;
            this.mealsService = mealsService;
            this.mealsFoodsService = mealsFoodsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> AddFood(int foodId, int mealId = 0)
        {
            var viewModel = await this.foodsService.GetByIdAsNoTrackingAsync<MealFoodInputModel>(foodId);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (mealId > 0)
            {
                viewModel.QuantityInGrams = await this.mealsFoodsService.GetQuantityAsync(foodId, mealId);
            }

            var userId = this.User.GetUserId();

            viewModel.UserId = userId;
            viewModel.HasMeals = await this.usersService.HasMealAsync(userId);

            this.ViewData[GlobalConstants.NameOfSelected] = mealId;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFood(MealFoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            if (await this.mealsService.IsExistAsync(model.MealId) == false
                || await this.foodsService.IsExistAsync(model.FoodId) == false)
            {
                return this.NotFound();
            }

            if (await this.mealsService.IsUserMealAsync(model.MealId, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.mealsFoodsService.AddAsync(model);

            return this.Redirect(GlobalConstants.MyMealsUrl);
        }

        public async Task<IActionResult> RemoveFood(int mealFoodId)
        {
            var mealFood = await this.mealsFoodsService.GetByIdAsync(mealFoodId);

            if (mealFood == null)
            {
                return this.NotFound();
            }

            if (mealFood.Meal.ForUserId != this.User.GetUserId())
            {
                return this.Unauthorized();
            }

            await this.mealsFoodsService.DeleteAsync(mealFood);

            return this.Redirect(GlobalConstants.MyMealsUrl);
        }
    }
}
