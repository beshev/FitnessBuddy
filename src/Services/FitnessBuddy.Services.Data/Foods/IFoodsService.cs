namespace FitnessBuddy.Services.Data.Foods
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Foods;

    public interface IFoodsService
    {
        public IEnumerable<TModel> GetAll<TModel>(int pageNumber, int itemsPerPage, string userId = null);

        public Food GetById(int id);

        public int GetCount(string userId = null);

        public bool Contains(int id);

        public Task<FoodInputModel> EditAsync(FoodInputModel model);

        public bool IsUserFood(string userId, int foodId);

        public Task DeleteAsync(int id);

        public Task AddAsync(string userId, FoodInputModel model);
    }
}
