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

        public async Task<FoodInputModel> EditAsync(FoodInputModel model)
        {
            var food = this.GetById(model.Id);

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
            var food = this.GetById(id);

            this.foodRepository.Delete(food);

            await this.foodRepository.SaveChangesAsync();
        }

        public IEnumerable<TModel> GetAll<TModel>(string userId = null, int skip = 0, int? take = null)
        {
            var query = this.foodRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(userId) == false)
            {
                query = query
                    .Where(x => x.AddedByUserId == userId);
            }

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

        public Food GetById(int id)
            => this.foodRepository
                 .All()
                 .Include(x => x.FoodName)
                 .FirstOrDefault(x => x.Id == id);

        public TModel GetByIdAsNoTracking<TModel>(int id)
        => this.foodRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        public bool IsUserFood(string userId, int foodId)
            => this.foodRepository
                .AllAsNoTracking()
                .Any(x => x.Id == foodId && x.AddedByUserId == userId);

        public bool Contains(int id)
            => this.foodRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id);

        public int GetCount(string userId = null)
        {
            var query = this.foodRepository
                .AllAsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.AddedByUserId == userId);
            }

            return query.Count();
        }
    }
}
