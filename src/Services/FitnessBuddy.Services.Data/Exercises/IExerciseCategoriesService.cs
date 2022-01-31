namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;

    public interface IExerciseCategoriesService
    {
        public IEnumerable<TModel> GetAll<TModel>();
    }
}
