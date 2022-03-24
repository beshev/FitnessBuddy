namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseCategoriesService : IExerciseCategoriesService
    {
        private readonly IDeletableEntityRepository<ExerciseCategory> exerciseCategoryRepository;

        public ExerciseCategoriesService(IDeletableEntityRepository<ExerciseCategory> exerciseCategoryRepository)
        {
            this.exerciseCategoryRepository = exerciseCategoryRepository;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>()
            => await this.exerciseCategoryRepository
            .AllAsNoTracking()
            .To<TModel>()
            .ToListAsync();

        public async Task<IEnumerable<TModel>> GetCategoryExercisesAsync<TModel>(string categoryName, int skip = 0, int? take = null)
        {
            var query = this.exerciseCategoryRepository
            .AllAsNoTracking()
            .Where(x => x.Name == categoryName)
            .SelectMany(x => x.Exercises);

            if (take.HasValue)
            {
                query = query.Skip(skip).Take(take.Value);
            }

            return await query
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<int> GetCategoryExercisesCountAsync(string categoryName)
            => await this.exerciseCategoryRepository
            .AllAsNoTracking()
            .Where(x => x.Name == categoryName)
            .SelectMany(x => x.Exercises)
            .CountAsync();
    }
}
