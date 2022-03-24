namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleCategoriesService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        public Task<IEnumerable<TModel>> GetCategoryArticlesAsync<TModel>(string categoryName);

        public Task<bool> IsExistAsync(string categoryName);
    }
}
