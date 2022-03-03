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

        public IEnumerable<TModel> GetAll<TModel>(int skip = 0, int? take = null)
        {
            IQueryable<Article> query = this.articlesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn);

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

        public bool IsExist(int id)
            => this.articlesRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id);

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
