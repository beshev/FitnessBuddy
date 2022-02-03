namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Exercises;

    public interface IExercisesService
    {
        public IEnumerable<ExerciseViewModel> GetAll(string search = null, int skip = 0, int? take = null);

        public int GetCount(string search = "");

        public TModel GetById<TModel>(int id);

        public Task AddAsync(string userId, ExerciseInputModel model);
    }
}
