namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Posts;

    public interface IPostsService
    {
        public IEnumerable<TModel> GetPostsByCategory<TModel>(int categoryId);

        public TModel GetById<TModel>(int id);

        public Task CreateAsync(PostInputModel model);

        public Task DeleteAsync(int id);

        public Task IncreaseViewsAsync(int id);
    }
}
