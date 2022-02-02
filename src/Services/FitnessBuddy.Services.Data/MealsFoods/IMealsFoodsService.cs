namespace FitnessBuddy.Services.Data.MealsFoodsService
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Meals;

    public interface IMealsFoodsService
    {
        public MealFood GetById(int mealFoodId);

        public bool Contains(int mealFoodId);

        public Task<MealFood> DeleteAsync(MealFood mealFood);

        public Task<MealFood> AddAsync(MealFoodInputModel model);
    }
}
