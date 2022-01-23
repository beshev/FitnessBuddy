namespace FitnessBuddy.Services.Data.Foods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Foods;
    using Microsoft.EntityFrameworkCore;

    public class FoodsService : IFoodsService
    {
        private readonly IDeletableEntityRepository<Food> foodRepository;
        private readonly IFoodNamesService foodNamesService;

        public FoodsService(
            IDeletableEntityRepository<Food> foodRepository,
            IFoodNamesService foodNamesService)
        {
            this.foodRepository = foodRepository;
            this.foodNamesService = foodNamesService;
        }

        public async Task AddAsync(string userId, FoodInputModel model)
        {
            var foodName = await this.foodNamesService.GetByNameAsync(model.Name);

            var food = AutoMapperConfig.MapperInstance.Map<FoodInputModel, Food>(model);
            food.AddedByUserId = userId;
            food.FoodNameId = foodName.Id;

            await this.foodRepository.AddAsync(food);
            await this.foodRepository.SaveChangesAsync();
        }

        public IEnumerable<FoodViewModel> GetAll()
            => this.foodRepository
            .AllAsNoTracking()
            .To<FoodViewModel>()
            .AsEnumerable();

        public Food GetFoodById(int id)
            => this.foodRepository
                 .All()
                 .Include(x => x.FoodName)
                 .FirstOrDefault(x => x.Id == id);

        public async Task<FoodInputModel> EditAsync(FoodInputModel model)
        {
            var food = this.GetFoodById(model.Id);

            var foodName = await this.foodNamesService.GetByNameAsync(model.Name);

            food.FoodNameId = foodName.Id;
            food.Description = model.Description;
            food.ProteinIn100Grams = model.ProteinIn100Grams;
            food.CarbohydratesIn100Grams = model.CarbohydratesIn100Grams;
            food.FatIn100Grams = model.FatIn100Grams;
            food.Sodium = model.Sodium;
            food.ImageUrl = model.ImageUrl;

            await this.foodRepository.SaveChangesAsync();

            return model;
        }

        public async Task DeleteAsync(int id)
        {
            var food = this.GetFoodById(id);

            this.foodRepository.Delete(food);

            await this.foodRepository.SaveChangesAsync();
        }

        public IEnumerable<FoodViewModel> FoodsAddedByUser(string userId)
            => this.foodRepository
                .AllAsNoTracking()
                .Where(x => x.AddedByUserId == userId)
                .To<FoodViewModel>()
                .AsEnumerable();
    }
}
