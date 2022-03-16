namespace FitnessBuddy.Services.Data.Foods
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Foods;

    public interface IFoodsService
    {
        public IEnumerable<TModel> GetAll<TModel>(string userId = null, string search = null, int skip = 0, int? take = null);

        public IEnumerable<TModel> GetRandom<TModel>(int count);

        public Food GetById(int id);

        public TModel GetByIdAsNoTracking<TModel>(int id);

        public int GetCount(string userId = null, string search = null);

        public bool IsExist(int id);

        public bool IsUserFood(string userId, int foodId);

        public Task<FoodInputModel> EditAsync(FoodInputModel model);

        public Task DeleteAsync(int id);

        public Task AddAsync(string userId, FoodInputModel model);
    }
}
