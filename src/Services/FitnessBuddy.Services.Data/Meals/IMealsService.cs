namespace FitnessBuddy.Services.Data.Meals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Meals;

    public interface IMealsService
    {
        public Meal GetById(int mealId);

        public Task<Meal> CreateAsync(string userId, MealInputModel model);

        public Task<Meal> DeleteAsync(int mealId);

        public bool IsExist(int mealId);

        public bool IsUserMeal(int mealId, string userId);

        public IEnumerable<TModel> GetUserMeals<TModel>(string userId);
    }
}
