namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;

    public interface IExerciseCategoriesService
    {
        public IEnumerable<TModel> GetAll<TModel>();

        public IEnumerable<TModel> GetCategoryExercises<TModel>(string categoryName);
    }
}
