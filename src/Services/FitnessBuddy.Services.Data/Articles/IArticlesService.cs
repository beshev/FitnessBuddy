namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Articles;

    public interface IArticlesService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>(int skip = 0, int? take = null);

        public Task<TModel> GetByIdAsync<TModel>(int id);

        public Task CreateAsync(ArticleInputModel model, string picturePath);

        public Task EditAsync(ArticleInputModel model, string picturePath);

        public Task DeleteAsync(int id);

        public Task<bool> IsExistAsync(int id);

        public Task<bool> IsUserCreatorAsync(string userId, int articleId);

        public Task<int> GetCountAsync();
    }
}
