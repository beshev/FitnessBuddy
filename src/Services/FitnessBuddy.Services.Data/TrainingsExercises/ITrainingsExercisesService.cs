namespace FitnessBuddy.Services.Data.TrainingsExercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsExercisesService
    {
        public Task AddAsync(TrainingExerciseInputModel model);

        public Task RemoveAsync(int id);

        public Task<bool> IsExistAsync(int id);

        public Task<bool> IsForUserAsync(int id, string userId);

        public Task<IEnumerable<TModel>> GetTrainingExercisesAsync<TModel>(int id);
    }
}
