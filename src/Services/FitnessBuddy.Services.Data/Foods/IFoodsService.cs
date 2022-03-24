namespace FitnessBuddy.Services.Data.Foods
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Foods;

    public interface IFoodsService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>(string userId = null, string search = null, int skip = 0, int? take = null);

        public Task<IEnumerable<TModel>> GetRandomAsync<TModel>(int count);

        public Task<Food> GetByIdAsync(int id);

        public Task<TModel> GetByIdAsNoTrackingAsync<TModel>(int id);

        public Task<int> GetCountAsync(string userId = null, string search = null);

        public Task<bool> IsExistAsync(int id);

        public Task<bool> IsUserFoodAsync(string userId, int foodId);

        public Task<FoodInputModel> EditAsync(FoodInputModel model);

        public Task DeleteAsync(int id);

        public Task AddAsync(string userId, FoodInputModel model);
    }
}
