namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Posts;

    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostsService(IDeletableEntityRepository<Post> postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task CreateAsync(PostInputModel model)
        {
            var post = AutoMapperConfig.MapperInstance.Map<Post>(model);

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public IEnumerable<TModel> GetPostsByCategory<TModel>(int categoryId)
            => this.postsRepository
            .AllAsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .To<TModel>()
            .AsEnumerable();
    }
}
