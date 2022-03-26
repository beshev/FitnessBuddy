namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostCategoriesService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        public Task<string> GetNameAsync(int categoryId);

        public Task<bool> IsExistAsync(int categoryId);
    }
}
