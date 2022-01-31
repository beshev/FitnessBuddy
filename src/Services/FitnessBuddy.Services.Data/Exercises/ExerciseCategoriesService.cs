namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Linq;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseCategoriesService : IExerciseCategoriesService
    {
        private readonly IDeletableEntityRepository<ExerciseCategory> exerciseCategoryRepository;

        public ExerciseCategoriesService(IDeletableEntityRepository<ExerciseCategory> exerciseCategoryRepository)
        {
            this.exerciseCategoryRepository = exerciseCategoryRepository;
        }

        public IEnumerable<TModel> GetAll<TModel>()
            => this.exerciseCategoryRepository
            .AllAsNoTracking()
            .To<TModel>()
            .AsEnumerable();
    }
}
