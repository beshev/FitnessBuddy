namespace FitnessBuddy.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task AddFoodToFavoriteAsync(string userId, Food food)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

            user.FavoriteUserFoods.Add(food);

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string userId, UserInputModel model)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

            user.UserName = model.UserName;
            user.WeightInKg = model.WeightInKg;
            user.GoalWeightInKg = model.GoalWeightInKg;
            user.ProfilePicture = model.ProfilePicture;
            user.HeightInCm = model.HeightInCm;
            user.DailyProteinGoal = model.DailyProteinGoal;
            user.DailyCarbohydratesGoal = model.DailyCarbohydratesGoal;
            user.DailyFatGoal = model.DailyFatGoal;
            user.AboutMe = model.AboutMe;

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task RemoveFoodFromFavoriteAsync(string userId, Food food)
        {
            var user = this.usersRepository
                .All()
                .Include(x => x.FavoriteUserFoods)
                .FirstOrDefault(x => x.Id == userId);

            user.FavoriteUserFoods.Remove(food);

            await this.usersRepository.SaveChangesAsync();
        }

        public IEnumerable<FoodViewModel> GetFavoriteFoods(string userId)
            => this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .SelectMany(x => x.FavoriteUserFoods)
                .To<FoodViewModel>()
                .AsEnumerable();

        public UserViewModel GetUserInfo(string userId)
            => this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .To<UserViewModel>()
                .FirstOrDefault();

        public bool IsFoodFavorite(string userId, int foodId)
            => this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .SelectMany(x => x.FavoriteUserFoods)
                .Any(x => x.Id == foodId);

        public bool HasMeal(string userId)
            => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .Any(x => x.Meals.Count > 0);
    }
}
