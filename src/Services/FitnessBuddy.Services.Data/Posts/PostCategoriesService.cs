namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class PostCategoriesService : IPostCategoriesService
    {
        private readonly IDeletableEntityRepository<PostCategory> postCategoriesRepository;

        public PostCategoriesService(IDeletableEntityRepository<PostCategory> postCategoriesRepository)
        {
            this.postCategoriesRepository = postCategoriesRepository;
        }

        public IEnumerable<TModel> GetAll<TModel>()
            => this.postCategoriesRepository
            .AllAsNoTracking()
            .To<TModel>()
            .AsEnumerable();

        public int GetCategoryPostsCount(int categoryId)
            => this.postCategoriesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == categoryId)
            .Count();

        public string GetName(int categoryId)
            => this.postCategoriesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == categoryId)
            .Select(x => x.Name)
            .FirstOrDefault();

        public bool IsExist(int categoryId)
            => this.postCategoriesRepository
            .AllAsNoTracking()
            .Any(x => x.Id == categoryId);
    }
}
