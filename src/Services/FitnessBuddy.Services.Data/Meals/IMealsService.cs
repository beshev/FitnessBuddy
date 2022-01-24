namespace FitnessBuddy.Services.Data.Meals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Meals;

    public interface IMealsService
    {
        public Task<Meal> CreateMealAsync(string userId, MealInputModel model);

        public Task<Meal> DeleteMealAsync(int mealId);

        public Task<MealFood> AddFoodToMealAsync(MealFoodInputModel model);

        public Task<MealFood> RemoveFoodFromMealAsync(int mealId);

        public IEnumerable<MealViewModel> GetAllMeals();
    }
}
