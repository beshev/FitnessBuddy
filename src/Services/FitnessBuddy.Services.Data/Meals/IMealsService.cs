namespace FitnessBuddy.Services.Data.Meals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Meals;

    public interface IMealsService
    {
        public Task<Meal> GetByIdAsync(int mealId);

        public Task<Meal> CreateAsync(string userId, MealInputModel model);

        public Task<Meal> DeleteAsync(int mealId);

        public Task<bool> IsExistAsync(int mealId);

        public Task<bool> IsUserMealAsync(int mealId, string userId);

        public Task<IEnumerable<TModel>> GetUserMealsAsync<TModel>(string userId);
    }
}
