namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExerciseCategoriesService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        public Task<IEnumerable<TModel>> GetCategoryExercisesAsync<TModel>(string categoryName, int skip = 0, int? take = null);

        public Task<int> GetCategoryExercisesCountAsync(string categoryName);
    }
}
