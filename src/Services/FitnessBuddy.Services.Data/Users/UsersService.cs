namespace FitnessBuddy.Services.Data.Users
{
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

        public IEnumerable<FoodViewModel> GetFavoriteFoods(string userId, int skip = 0, int? take = null)
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

            return query
                .To<FoodViewModel>()
                .AsEnumerable();
        }

        public TViewModel GetUserInfo<TViewModel>(string userId)
            => this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .To<TViewModel>()
                .FirstOrDefault();

        public bool IsFoodFavorite(string userId, int foodId)
            => this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .SelectMany(x => x.FavoriteFoods)
                .Any(x => x.Id == foodId);

        public bool HasMeal(string userId)
            => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .Any(x => x.Meals.Count > 0);

        public bool IsUsernameExist(string username)
            => this.usersRepository
            .AllAsNoTracking()
            .Any(x => x.UserName == username);

        public int FavoriteFoodsCount(string userId)
            => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.FavoriteFoods)
            .Count();

        public ProfileViewModel GetProfileData(string userId)
        {
            var userInfo = this.GetUserInfo<UserViewModel>(userId);
            var userMeals = this.mealsService.GetUserMeals<MealViewModel>(userId);

            var profileInfo = AutoMapperConfig.MapperInstance.Map<IEnumerable<MealViewModel>, ProfileViewModel>(userMeals);
            profileInfo.UserInfo = userInfo;

            return profileInfo;
        }

        public string GetIdByUsername(string username)
            => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.UserName == username)
            .Select(x => x.Id)
            .FirstOrDefault();

        public IEnumerable<TModel> GetAll<TModel>(string username = "")
        {
            var query = this.usersRepository
            .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(username) == false)
            {
                query = query
                    .Where(x => x.UserName == username);
            }

            return query.To<TModel>()
            .AsEnumerable();
        }

        public IEnumerable<TModel> GetFollowers<TModel>(string userId)
            => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Followers)
            .To<TModel>()
            .AsEnumerable();

        public IEnumerable<TModel> GetFollowing<TModel>(string userId)
        => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Following)
            .To<TModel>()
            .AsEnumerable();

        public int GetCount()
            => this.usersRepository
            .AllAsNoTracking()
            .Count();
    }
}
