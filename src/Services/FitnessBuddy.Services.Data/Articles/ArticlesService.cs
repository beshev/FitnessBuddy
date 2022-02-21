namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Articles;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;

        public ArticlesService(IDeletableEntityRepository<Article> articlesRepository)
        {
            this.articlesRepository = articlesRepository;
        }

        public async Task CreateAsync(ArticleInputModel model)
        {
            var article = AutoMapperConfig.MapperInstance.Map<Article>(model);

            await this.articlesRepository.AddAsync(article);
            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var article = this.articlesRepository
                   .All()
                   .FirstOrDefault(x => x.Id == id);

            this.articlesRepository.Delete(article);
            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ArticleInputModel model)
        {
            var article = this.articlesRepository
                .All()
                .FirstOrDefault(x => x.Id == model.Id);

            article.Title = model.Title;
            article.Content = model.Content;
            article.CategoryId = model.CategoryId.Value;
            article.ImageUrl = model.ImageUrl;

            await this.articlesRepository.SaveChangesAsync();
        }

        public IEnumerable<TModel> GetAll<TModel>(int skip = 0, int? take = null)
        {
            var query = this.articlesRepository
                .AllAsNoTracking();

            if (take.HasValue)
            {
                query = query
                    .Skip(skip)
                    .Take(take.Value);
            }

            return query
                .To<TModel>()
                .AsEnumerable();
        }

        public TModel GetById<TModel>(int id)
            => this.articlesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        public bool IsUserCreator(string userId, int articleId)
            => this.articlesRepository
            .AllAsNoTracking()
            .Any(x => x.Id == articleId && x.CreatorId == userId);

        public int GetCount()
            => this.articlesRepository
                .AllAsNoTracking()
                .Count();
    }
}
