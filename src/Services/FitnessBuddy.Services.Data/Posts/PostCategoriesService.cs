namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class PostCategoriesService : IPostCategoriesService
    {
        private readonly IDeletableEntityRepository<PostCategory> postCategoriesRepository;

        public PostCategoriesService(IDeletableEntityRepository<PostCategory> postCategoriesRepository)
        {
            this.postCategoriesRepository = postCategoriesRepository;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>()
            => await this.postCategoriesRepository
            .AllAsNoTracking()
            .To<TModel>()
            .ToListAsync();

        public async Task<int> GetCategoryPostsCountAsync(int categoryId)
            => await this.postCategoriesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == categoryId)
            .CountAsync();

        public async Task<string> GetNameAsync(int categoryId)
            => await this.postCategoriesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == categoryId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();

        public async Task<bool> IsExistAsync(int categoryId)
            => await this.postCategoriesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == categoryId);
    }
}
