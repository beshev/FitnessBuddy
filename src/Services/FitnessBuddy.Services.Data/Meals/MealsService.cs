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

        public MealsService(
            IDeletableEntityRepository<Meal> mealRepository)
        {
            this.mealRepository = mealRepository;
        }

        public bool Contains(int mealId)
            => this.mealRepository
            .AllAsNoTracking()
            .Any(x => x.Id == mealId);

        public async Task<Meal> CreateAsync(string userId, MealInputModel model)
        {
            var meal = AutoMapperConfig.MapperInstance.Map<MealInputModel, Meal>(model);
            meal.ForUserId = userId;

            await this.mealRepository.AddAsync(meal);
            await this.mealRepository.SaveChangesAsync();

            return meal;
        }

        public async Task<Meal> DeleteAsync(int mealId)
        {
            var meal = this.mealRepository
                .All()
                .FirstOrDefault(x => x.Id == mealId);

            this.mealRepository.Delete(meal);

            await this.mealRepository.SaveChangesAsync();

            return meal;
        }

        public Meal GetById(int mealId)
            => this.mealRepository
            .AllAsNoTracking()
            .FirstOrDefault(x => x.Id == mealId);

        public IEnumerable<TViewModel> GetUserMeals<TViewModel>(string userId)
            => this.mealRepository
            .AllAsNoTracking()
            .Where(x => x.ForUserId == userId)
            .To<TViewModel>()
            .AsEnumerable();
    }
}
