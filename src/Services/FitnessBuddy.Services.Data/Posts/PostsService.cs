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

        public Task DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public TModel GetById<TModel>(int id)
            => this.postsRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        public IEnumerable<TModel> GetPostsByCategory<TModel>(int categoryId)
            => this.postsRepository
            .AllAsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .To<TModel>()
            .AsEnumerable();

        public async Task IncreaseViewsAsync(int id)
        {
            var post = this.GetById(id);

            post.Views++;

            await this.postsRepository.SaveChangesAsync();
        }

        private Post GetById(int id)
            => this.postsRepository
            .All()
            .FirstOrDefault(x => x.Id == id);
    }
}
