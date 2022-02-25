namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;

    public interface IArticleCategoriesService
    {
        public IEnumerable<TModel> GetAll<TModel>();

        public IEnumerable<TModel> GetCategoryArticles<TModel>(string categoryName);

        public bool IsExist(string categoryName);
    }
}
