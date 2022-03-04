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

        public async Task DeleteAsync(int id)
        {
            var post = this.GetById(id);

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(PostInputModel model)
        {
            var post = this.GetById(model.Id);

            post.Title = model.Title;
            post.Description = model.Description;
            post.CategoryId = model.CategoryId;

            await this.postsRepository.SaveChangesAsync();
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
            .OrderByDescending(x => x.Views)
            .ThenByDescending(x => x.CreatedOn)
            .To<TModel>()
            .AsEnumerable();

        public async Task IncreaseViewsAsync(int id)
        {
            var post = this.GetById(id);

            post.Views++;

            await this.postsRepository.SaveChangesAsync();
        }

        public bool IsExist(int id)
            => this.postsRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id);

        public int GetCount()
            => this.postsRepository
            .AllAsNoTracking()
            .Count();

        public bool IsUserAuthor(int postId, string userId)
            => this.postsRepository
            .AllAsNoTracking()
            .Any(x => x.Id == postId && x.AuthorId == userId);

        private Post GetById(int id)
            => this.postsRepository
            .All()
            .FirstOrDefault(x => x.Id == id);

        public IEnumerable<TModel> GetAll<TModel>()
            => this.postsRepository
            .AllAsNoTracking()
            .To<TModel>()
            .AsEnumerable();
    }
}
