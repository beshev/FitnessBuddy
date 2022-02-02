namespace FitnessBuddy.Services.Data.TrainingsExercises
{
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Exercises;

    public interface ITrainingsExercisesService
    {
        public Task AddAsync(ExerciseTrainingInputModel model);
    }
}
