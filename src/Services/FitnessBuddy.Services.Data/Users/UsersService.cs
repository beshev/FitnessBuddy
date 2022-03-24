namespace FitnessBuddy.Services.Data.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IMealsService mealsService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IMealsService mealsService)
        {
            this.usersRepository = usersRepository;
            this.mealsService = mealsService;
        }

        public async Task AddFoodToFavoriteAsync(string userId, Food food)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

            user.FavoriteFoods.Add(food);

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string userId, UserInputModel model, string picturePath)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

            user.Gender = model.Gender;
            user.WeightInKg = model.WeightInKg;
            user.GoalWeightInKg = model.GoalWeightInKg;
            user.HeightInCm = model.HeightInCm;
            user.DailyProteinGoal = model.DailyProteinGoal;
            user.DailyCarbohydratesGoal = model.DailyCarbohydratesGoal;
            user.DailyFatGoal = model.DailyFatGoal;
            user.AboutMe = model.AboutMe;

            if (model.ProfilePicture != null)
            {
                Directory.CreateDirectory($"{picturePath}/profileimages/");
                var physicalPath = $"{picturePath}/profileimages/{userId}{Path.GetExtension(model.ProfilePicture.FileName)}";

                using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }

                user.ProfilePicture = physicalPath;
            }

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task RemoveFoodFromFavoriteAsync(string userId, Food food)
        {
            var user = this.usersRepository
                .All()
                .Include(x => x.FavoriteFoods)
                .FirstOrDefault(x => x.Id == userId);

            user.FavoriteFoods.Remove(food);

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<FoodViewModel>> GetFavoriteFoodsAsync(string userId, int skip = 0, int? take = null)
        {
            var query = this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .SelectMany(x => x.FavoriteFoods);

            if (take.HasValue)
            {
                query = query
                    .Skip(skip)
                    .Take(take.Value);
            }

            return await query
                .To<FoodViewModel>()
                .ToListAsync();
        }

        public async Task<TModel> GetUserInfoAsync<TModel>(string userId)
            => await this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .To<TModel>()
                .FirstOrDefaultAsync();

        public async Task<bool> IsFoodFavoriteAsync(string userId, int foodId)
            => await this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .SelectMany(x => x.FavoriteFoods)
                .AnyAsync(x => x.Id == foodId);

        public async Task<bool> HasMealAsync(string userId)
            => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .AnyAsync(x => x.Meals.Count > 0);

        public async Task<bool> IsUsernameExistAsync(string username)
            => await this.usersRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.UserName == username);

        public async Task<int> FavoriteFoodsCountAsync(string userId)
            => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.FavoriteFoods)
            .CountAsync();

        public async Task<ProfileViewModel> GetProfileDataAsync(string userId)
        {
            var userInfo = await this.GetUserInfoAsync<UserViewModel>(userId);
            var userMeals = await this.mealsService.GetUserMealsAsync<MealViewModel>(userId);

            var profileInfo = AutoMapperConfig.MapperInstance.Map<IEnumerable<MealViewModel>, ProfileViewModel>(userMeals);
            profileInfo.UserInfo = userInfo;

            return profileInfo;
        }

        public async Task<string> GetIdByUsernameAsync(string username)
            => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.UserName == username)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string username = "", int skip = 0, int? take = null)
        {
            IQueryable<ApplicationUser> query = this.usersRepository
            .AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn);

            if (string.IsNullOrWhiteSpace(username) == false)
            {
                query = query
                    .Where(x => x.UserName == username);
            }

            if (take.HasValue)
            {
                query = query
                    .Skip(skip)
                    .Take(take.Value);
            }

            return await query.To<TModel>()
            .ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetFollowersAsync<TModel>(string userId)
            => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Followers)
            .To<TModel>()
            .ToListAsync();

        public async Task<IEnumerable<TModel>> GetFollowingAsync<TModel>(string userId)
        => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Following)
            .To<TModel>()
            .ToListAsync();

        public async Task<int> GetCountAsync()
            => await this.usersRepository
            .AllAsNoTracking()
            .CountAsync();

        public async Task BanUserAsync(string username, string banReason)
        {
            var user = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserName == username);

            user.BannedOn = DateTime.UtcNow;
            user.IsBanned = true;
            user.BanReason = banReason;

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task UnbanUserAsync(string username)
        {
            var user = await this.usersRepository
                   .All()
                   .FirstOrDefaultAsync(x => x.UserName == username);

            user.IsBanned = false;
            user.BanReason = string.Empty;
            user.BannedOn = null;

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task<bool> IsUserBannedAsync(string userId)
            => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => x.IsBanned)
            .FirstOrDefaultAsync();

        public  async Task<string> GetUsernameByIdAsync(string userId)
            => await this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => x.UserName)
            .FirstOrDefaultAsync();
    }
}
