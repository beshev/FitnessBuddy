namespace FitnessBuddy.Services.Data.Meals
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Meals;

    public class MealsService : IMealsService
    {
        private readonly IDeletableEntityRepository<Meal> mealRepository;
        private readonly IDeletableEntityRepository<MealFood> mealFoodRepository;

        public MealsService(
            IDeletableEntityRepository<Meal> mealRepository,
            IDeletableEntityRepository<MealFood> mealFoodRepository)
        {
            this.mealRepository = mealRepository;
            this.mealFoodRepository = mealFoodRepository;
        }

        public async Task<MealFood> AddFoodToMealAsync(MealFoodInputModel model)
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

            await this.mealRepository.SaveChangesAsync();

            return mealFood;
        }

        public async Task<Meal> CreateMealAsync(string userId, MealInputModel model)
        {
            var meal = AutoMapperConfig.MapperInstance.Map<MealInputModel, Meal>(model);
            meal.ForUserId = userId;

            await this.mealRepository.AddAsync(meal);
            await this.mealRepository.SaveChangesAsync();

            return meal;
        }

        public async Task<Meal> DeleteMealAsync(int mealId)
        {
            var meal = this.mealRepository
                .All()
                .FirstOrDefault(x => x.Id == mealId);

            this.mealRepository.Delete(meal);
            await this.mealRepository.SaveChangesAsync();

            return meal;
        }

        public IEnumerable<MealViewModel> GetAllMeals()
            => this.mealRepository
            .AllAsNoTracking()
            .To<MealViewModel>()
            .AsEnumerable();

        public async Task<MealFood> RemoveFoodFromMealAsync(int mealFoodId)
        {
            var mealFood = this.mealFoodRepository
                .All()
                .FirstOrDefault(x => x.Id == mealFoodId);

            this.mealFoodRepository.Delete(mealFood);

            await this.mealFoodRepository.SaveChangesAsync();

            return mealFood;
        }
    }
}
