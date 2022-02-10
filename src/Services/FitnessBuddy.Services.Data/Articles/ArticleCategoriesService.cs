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
    }
}
