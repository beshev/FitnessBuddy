namespace FitnessBuddy.Services.Data.MealsFoodsService
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Meals;

    public interface IMealsFoodsService
    {
        public Task<MealFood> GetByIdAsync(int mealFoodId);

        public Task<bool> ContainsAsync(int mealFoodId);

        public Task<MealFood> DeleteAsync(MealFood mealFood);

        public Task<MealFood> AddAsync(MealFoodInputModel model);
    }
}
