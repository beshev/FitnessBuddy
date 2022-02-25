namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ArticleCategoriesService : IArticleCategoriesService
    {
        private readonly IDeletableEntityRepository<ArticleCategory> articleCategoriesRepository;

        public ArticleCategoriesService(IDeletableEntityRepository<ArticleCategory> articleCategoriesRepository)
        {
            this.articleCategoriesRepository = articleCategoriesRepository;
        }

        public IEnumerable<TModel> GetAll<TModel>()
            => this.articleCategoriesRepository
            .AllAsNoTracking()
            .To<TModel>()
            .AsEnumerable();

        public IEnumerable<TModel> GetCategoryArticles<TModel>(string categoryName)
            => this.articleCategoriesRepository
            .AllAsNoTracking()
            .Where(x => x.Name == categoryName)
            .SelectMany(x => x.Articles)
            .To<TModel>()
            .AsEnumerable();

        public bool IsExist(string categoryName)
            => this.articleCategoriesRepository
            .AllAsNoTracking()
            .Any(x => x.Name == categoryName);
    }
}
