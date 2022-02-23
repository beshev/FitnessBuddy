namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;

    public interface IPostCategoriesService
    {
        public IEnumerable<TModel> GetAll<TModel>();
    }
}
