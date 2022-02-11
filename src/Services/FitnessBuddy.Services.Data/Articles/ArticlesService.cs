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

        public IEnumerable<TModel> GetAll<TModel>(string search = null, int skip = 0, int? take = null)
        {
            var query = this.articlesRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query.Where(x => x.Title == search);
            }

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

        public int GetCount(string search = "")
        {
            var query = this.articlesRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query.Where(x => x.Title == search);
            }

            return query.Count();
        }
    }
}
