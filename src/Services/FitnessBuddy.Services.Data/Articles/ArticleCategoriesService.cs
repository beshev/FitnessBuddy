namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ArticleCategoriesService : IArticleCategoriesService
    {
        private readonly IDeletableEntityRepository<ArticleCategory> articleCategoriesRepository;

        public ArticleCategoriesService(IDeletableEntityRepository<ArticleCategory> articleCategoriesRepository)
        {
            this.articleCategoriesRepository = articleCategoriesRepository;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>()
            => await this.articleCategoriesRepository
            .AllAsNoTracking()
            .To<TModel>()
            .ToListAsync();

        public async Task<IEnumerable<TModel>> GetCategoryArticlesAsync<TModel>(string categoryName)
            => await this.articleCategoriesRepository
            .AllAsNoTracking()
            .Where(x => x.Name == categoryName)
            .SelectMany(x => x.Articles)
            .To<TModel>()
            .ToListAsync();

        public async Task<bool> IsExistAsync(string categoryName)
            => await this.articleCategoriesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Name == categoryName);
    }
}
