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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealsFoodsController : Controller
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

        public IActionResult AddFood(int foodId, int mealId = 0)
        {
            var viewModel = this.foodsService.GetByIdAsNoTracking<MealFoodInputModel>(foodId);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();

            viewModel.UserId = userId;
            viewModel.HasMeals = this.usersService.HasMeal(userId);

            this.ViewData["Selected"] = mealId;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFood(MealFoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            if (this.mealsService.Contains(model.MealId) == false
                || this.foodsService.Contains(model.FoodId) == false)
            {
                return this.NotFound();
            }

            await this.mealsFoodsService.AddAsync(model);

            return this.Redirect(GlobalConstants.MyMealsUrl);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFood(int mealFoodId)
        {
            var mealFood = this.mealsFoodsService.GetById(mealFoodId);

            if (mealFood == null || mealFood.Meal.ForUserId != this.User.GetUserId())
            {
                return this.NotFound();
            }

            await this.mealsFoodsService.DeleteAsync(mealFood);

            return this.Redirect(GlobalConstants.MyMealsUrl);
        }
    }
}
