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

        public async Task<MealFood> Add(MealFoodInputModel model)
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

            mealFood.QuantityInGrams += model.QuantityInGrams;

            await this.mealFoodRepository.SaveChangesAsync();

            return mealFood;
        }

        public bool Contains(int mealFoodId)
            => this.mealFoodRepository
            .AllAsNoTracking()
            .Any(x => x.Id == mealFoodId);

        public MealFood GetById(int mealFoodId)
            => this.mealFoodRepository
            .All()
            .Include(x => x.Meal)
            .FirstOrDefault(x => x.Id == mealFoodId);

        public async Task<MealFood> Remove(MealFood mealFood)
        {
            this.mealFoodRepository.Delete(mealFood);

            await this.mealFoodRepository.SaveChangesAsync();

            return mealFood;
        }
    }
}
