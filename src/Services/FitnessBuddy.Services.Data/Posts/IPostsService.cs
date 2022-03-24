namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Posts;

    public interface IPostsService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>(int? categoryId = null, int skip = 0, int? take = null);

        public Task<TModel> GetByIdAsync<TModel>(int id);

        public Task<int> GetCountAsync(int? categoryId = null);

        public Task<bool> IsExistAsync(int id);

        public Task<bool> IsUserAuthorAsync(int postId, string userId);

        public Task CreateAsync(PostInputModel model);

        public Task DeleteAsync(int id);

        public Task EditAsync(PostInputModel model);

        public Task IncreaseViewsAsync(int id);
    }
}
