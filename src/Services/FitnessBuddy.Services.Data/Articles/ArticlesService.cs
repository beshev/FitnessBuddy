namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Cloudinary;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Articles;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public ArticlesService(
            IDeletableEntityRepository<Article> articlesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.articlesRepository = articlesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task CreateAsync(ArticleInputModel model)
        {
            var article = AutoMapperConfig.MapperInstance.Map<Article>(model);

            await this.articlesRepository.AddAsync(article);
            article.ImageUrl = string.Empty;
            await this.articlesRepository.SaveChangesAsync();

            var cloudFolder = $"articles/{article.Id}";

            // TODO: Find another way for saving the new picture. !!
            article.ImageUrl = await this.cloudinaryService.UploadAsync(model.Picture, cloudFolder);
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

            var cloudFolder = $"articles/{article.Id}";

            article.Title = model.Title;
            article.Content = model.Content;
            article.CategoryId = model.CategoryId.Value;
            article.ImageUrl = await this.cloudinaryService.UploadAsync(model.Picture, cloudFolder);

            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(int skip = 0, int? take = null)
        {
            IQueryable<Article> query = this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.ArticleRatings.Any() ? x.ArticleRatings.Average(x => x.Rating) : 0)
                .ThenByDescending(x => x.CreatedOn);

            if (take.HasValue)
            {
                query = query
                    .Skip(skip)
                    .Take(take.Value);
            }

            return await query
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.articlesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefaultAsync();

        public async Task<bool> IsUserCreatorAsync(string userId, int articleId)
            => await this.articlesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == articleId && x.CreatorId == userId);

        public async Task<int> GetCountAsync()
            => await this.articlesRepository
                .AllAsNoTracking()
                .CountAsync();

        public async Task<bool> IsExistAsync(int id)
            => await this.articlesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id);
    }
}
