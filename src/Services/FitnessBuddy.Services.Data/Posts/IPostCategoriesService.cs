namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;

    public interface IPostCategoriesService
    {
        public IEnumerable<TModel> GetAll<TModel>();

        public int GetCategoryPostsCount(int categoryId);

        public string GetName(int categoryId);

        public bool IsExist(int categoryId);
    }
}
