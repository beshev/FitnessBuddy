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

        public int GetCount(int? categoryId = null)
        {
            var query = this.postsRepository
            .AllAsNoTracking();

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            return query.Count();
        }

        public bool IsUserAuthor(int postId, string userId)
            => this.postsRepository
            .AllAsNoTracking()
            .Any(x => x.Id == postId && x.AuthorId == userId);

        public IEnumerable<TModel> GetAll<TModel>(int? categoryId = null, int skip = 0, int? take = null)
        {
            IQueryable<Post> query = this.postsRepository
            .AllAsNoTracking();

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            query = query.OrderByDescending(x => x.Views)
            .ThenByDescending(x => x.CreatedOn);

            if (take.HasValue)
            {
                query = query.Skip(skip).Take(take.Value);
            }

            return query.To<TModel>()
                .AsEnumerable();
        }

        private Post GetById(int id)
            => this.postsRepository
            .All()
            .FirstOrDefault(x => x.Id == id);
    }
}
