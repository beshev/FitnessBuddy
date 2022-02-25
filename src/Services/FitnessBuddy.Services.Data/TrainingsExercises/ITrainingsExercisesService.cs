namespace FitnessBuddy.Services.Data.TrainingsExercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsExercisesService
    {
        public Task AddAsync(TrainingExerciseInputModel model);

        public Task RemoveAsync(int id);

        public bool IsExist(int id);

        public bool IsForUser(int id, string userId);

        public IEnumerable<TModel> GetTrainingExercises<TModel>(int id);
    }
}
