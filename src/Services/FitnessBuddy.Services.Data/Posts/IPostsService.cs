namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Posts;

    public interface IPostsService
    {
        public IEnumerable<TModel> GetPostsByCategory<TModel>(int categoryId);

        public Task CreateAsync(PostInputModel model);
    }
}
