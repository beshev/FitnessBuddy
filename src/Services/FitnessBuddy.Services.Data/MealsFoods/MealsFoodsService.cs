namespace FitnessBuddy.Services.Data.MealsFoodsService
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Meals;
    using Microsoft.EntityFrameworkCore;

    public class MealsFoodsService : IMealsFoodsService
    {
        private readonly IDeletableEntityRepository<MealFood> mealFoodRepository;

        public MealsFoodsService(IDeletableEntityRepository<MealFood> mealFoodRepository)
        {
            this.mealFoodRepository = mealFoodRepository;
        }

        public async Task<MealFood> AddAsync(MealFoodInputModel model)
        {
            MealFood mealFood = this.mealFoodRepository
                   .All()
                   .FirstOrDefault(x => x.FoodId == model.FoodId && x.MealId == model.MealId);

            if (mealFood == null)
            {
                mealFood = new MealFood
                {
                    FoodId = model.FoodId,
                    MealId = model.MealId,
                    QuantityInGrams = 0,
                };

                await this.mealFoodRepository.AddAsync(mealFood);
            }

            mealFood.QuantityInGrams = model.QuantityInGrams;

            await this.mealFoodRepository.SaveChangesAsync();

            return mealFood;
        }

        public async Task<bool> ContainsAsync(int mealFoodId)
            => await this.mealFoodRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == mealFoodId);

        public async Task<MealFood> GetByIdAsync(int mealFoodId)
            => await this.mealFoodRepository
            .All()
            .Include(x => x.Meal)
            .FirstOrDefaultAsync(x => x.Id == mealFoodId);

        public async Task<MealFood> DeleteAsync(MealFood mealFood)
        {
            this.mealFoodRepository.Delete(mealFood);

            await this.mealFoodRepository.SaveChangesAsync();

            return mealFood;
        }

        public async Task<double> GetQuantityAsync(int foodId, int mealId)
            => await this.mealFoodRepository
            .AllAsNoTracking()
            .Where(x => x.FoodId == foodId && x.MealId == mealId)
            .Select(x => x.QuantityInGrams)
            .FirstOrDefaultAsync();
    }
}
