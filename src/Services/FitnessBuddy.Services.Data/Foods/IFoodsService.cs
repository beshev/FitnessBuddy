namespace FitnessBuddy.Services.Data.Foods
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Foods;

    public interface IFoodsService
    {
        public IEnumerable<FoodViewModel> GetAll();

        public Food GetFoodById(int id);

        public Task<FoodInputModel> EditAsync(FoodInputModel model);

        public IEnumerable<FoodViewModel> FoodsAddedByUser(string userId);

        public Task DeleteAsync(int id);

        public Task AddAsync(string userId, FoodInputModel model);
    }
}
