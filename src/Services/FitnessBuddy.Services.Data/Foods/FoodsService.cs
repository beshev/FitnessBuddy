namespace FitnessBuddy.Services.Data.Foods
{
    using System;
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

            var food = AutoMapperConfig.MapperInstance.Map<Food>(model);
            food.AddedByUserId = userId;
            food.FoodNameId = foodName.Id;

            await this.foodRepository.AddAsync(food);
            await this.foodRepository.SaveChangesAsync();
        }

        public async Task<FoodInputModel> EditAsync(FoodInputModel model)
        {
            var food = await this.GetByIdAsync(model.Id);

            var foodName = await this.foodNamesService.GetByNameAsync(model.Name);

            food.FoodNameId = foodName.Id;
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
            var food = await this.GetByIdAsync(id);

            this.foodRepository.Delete(food);

            await this.foodRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string userId = null, string search = null, int skip = 0, int? take = null)
        {
            var query = this.foodRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(userId) == false)
            {
                query = query.Where(x => x.AddedByUserId == userId);
            }

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query.Where(x => x.FoodName.Name.Contains(search));
            }

            query = query.OrderByDescending(x => x.CreatedOn);

            if (take.HasValue)
            {
                query = query
                    .Skip(skip)
                    .Take(take.Value);
            }

            return await query
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<Food> GetByIdAsync(int id)
            => await this.foodRepository
                 .All()
                 .Include(x => x.FoodName)
                 .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<TModel> GetByIdAsNoTrackingAsync<TModel>(int id)
        => await this.foodRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefaultAsync();

        public async Task<bool> IsUserFoodAsync(string userId, int foodId)
            => await this.foodRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == foodId && x.AddedByUserId == userId);

        public async Task<bool> IsExistAsync(int id)
            => await this.foodRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id);

        public async Task<int> GetCountAsync(string userId = null, string search = null)
        {
            var query = this.foodRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(userId) == false)
            {
                query = query.Where(x => x.AddedByUserId == userId);
            }

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query.Where(x => x.FoodName.Name.Contains(search));
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<TModel>> GetRandomAsync<TModel>(int count)
            => await this.foodRepository
            .AllAsNoTracking()
            .OrderBy(x => Guid.NewGuid())
            .To<TModel>()
            .Take(count)
            .ToListAsync();
    }
}
