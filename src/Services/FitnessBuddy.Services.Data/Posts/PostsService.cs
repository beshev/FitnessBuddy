namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Posts;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.postsRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefaultAsync();

        public async Task IncreaseViewsAsync(int id)
        {
            var post = this.GetById(id);

            post.Views++;

            await this.postsRepository.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int id)
            => await this.postsRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id);

        public async Task<int> GetCountAsync(int? categoryId = null)
        {
            var query = this.postsRepository
            .AllAsNoTracking();

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<bool> IsUserAuthorAsync(int postId, string userId)
            => await this.postsRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == postId && x.AuthorId == userId);

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(int? categoryId = null, int skip = 0, int? take = null)
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

            return await query.To<TModel>()
                .ToListAsync();
        }

        private Post GetById(int id)
            => this.postsRepository
            .All()
            .FirstOrDefault(x => x.Id == id);
    }
}
