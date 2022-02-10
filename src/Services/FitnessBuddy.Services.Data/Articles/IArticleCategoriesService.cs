namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;

    public interface IArticleCategoriesService
    {
        public IEnumerable<TModel> GetAll<TModel>();
    }
}
