namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Meals;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealsController : Controller
    {
        private readonly IMealsService mealsService;

        public MealsController(IMealsService mealsService)
        {
            this.mealsService = mealsService;
        }

        [Authorize]
        public IActionResult MyMeals()
        {
            string userId = this.User.GetUserId();
            var viewModel = this.mealsService.GetUserMeals(userId);

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult AddFood(int id, string foodName)
        {
            var viewModel = new MealFoodInputModel
            {
                FoodId = id,
                FoodName = foodName,
            };

            // TODO: Make ViewComponent for meals selection
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFood(MealFoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.mealsService.AddFoodToMealAsync(model);

            return this.RedirectToAction(nameof(this.MyMeals));
        }

        [Authorize]
        public async Task<IActionResult> RemoveFood(int mealFoodId)
        {
            await this.mealsService.RemoveFoodFromMealAsync(mealFoodId);

            return this.RedirectToAction(nameof(this.MyMeals));
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(MealInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();
            await this.mealsService.CreateMealAsync(userId, model);

            return this.RedirectToAction(nameof(this.MyMeals));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int mealId)
        {
            await this.mealsService.DeleteMealAsync(mealId);

            return this.RedirectToAction(nameof(this.MyMeals));
        }
    }
}
