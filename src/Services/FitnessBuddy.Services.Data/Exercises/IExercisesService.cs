namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Exercises;

    public interface IExercisesService
    {
        public Task<IEnumerable<ExerciseViewModel>> GetAllAsync(string search = null, int skip = 0, int? take = null);

        public Task<IEnumerable<TModel>> GetRandomAsync<TModel>(int count);

        public Task EditAsync(ExerciseInputModel model);

        public Task DeleteAsync(int id);

        public Task<int> GetCountAsync(string search = "");

        public Task<bool> IsUserCreatorAsync(string userId, int exerciseId);

        public Task<bool> IsExistAsync(int id);

        public Task<TModel> GetByIdAsync<TModel>(int id);

        public Task<int> AddAsync(string userId, ExerciseInputModel model);
    }
}
