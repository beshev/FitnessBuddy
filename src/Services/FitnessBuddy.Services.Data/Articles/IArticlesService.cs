namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Articles;

    public interface IArticlesService
    {
        public IEnumerable<TModel> GetAll<TModel>(int skip = 0, int? take = null);

        public TModel GetById<TModel>(int id);

        public Task CreateAsync(ArticleInputModel model);

        public Task EditAsync(ArticleInputModel model);

        public Task DeleteAsync(int id);

        public bool IsUserCreator(string userId, int articleId);

        public int GetCount();
    }
}
