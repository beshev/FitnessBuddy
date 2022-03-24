namespace FitnessBuddy.Services.Data.Articles
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesService : IArticlesService
    {
        private readonly IDeletableEntityRepository<Article> articlesRepository;

        public ArticlesService(IDeletableEntityRepository<Article> articlesRepository)
        {
            this.articlesRepository = articlesRepository;
        }

        public async Task CreateAsync(ArticleInputModel model, string picturePath)
        {
            var article = AutoMapperConfig.MapperInstance.Map<Article>(model);

            await this.articlesRepository.AddAsync(article);
            article.ImageUrl = string.Empty;
            await this.articlesRepository.SaveChangesAsync();

            // TODO: Find another way for saving the new picture. !!
            article.ImageUrl = await SavePictureAsync(model.Picture, article.Id, picturePath);
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

        public async Task EditAsync(ArticleInputModel model, string picturePath)
        {
            var article = this.articlesRepository
                .All()
                .FirstOrDefault(x => x.Id == model.Id);

            article.Title = model.Title;
            article.Content = model.Content;
            article.CategoryId = model.CategoryId.Value;
            article.ImageUrl = await SavePictureAsync(model.Picture, article.Id, picturePath);

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

        private static async Task<string> SavePictureAsync(IFormFile picture, int articleId, string picturePath)
        {
            Directory.CreateDirectory($@"{picturePath}\articles\");
            var physicalPath = $@"{picturePath}\articles\{articleId}{Path.GetExtension(picture.FileName)}";

            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }

            return physicalPath;
        }
    }
}
