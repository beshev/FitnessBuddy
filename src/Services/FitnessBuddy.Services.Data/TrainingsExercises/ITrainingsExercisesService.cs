namespace FitnessBuddy.Services.Data.TrainingsExercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Exercises;

    public interface ITrainingsExercisesService
    {
        public Task AddAsync(ExerciseTrainingInputModel model);

        public Task RemoveAsync(int id);

        public IEnumerable<TModel> GetTrainingExercises<TModel>(int id);
    }
}
