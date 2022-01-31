﻿namespace FitnessBuddy.Services.Data.Users
{
    using System.Collections.Generic;
    using System.IO;
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

            user.FavoriteFoods.Add(food);

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string userId, UserInputModel model, string picturePath)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

            user.UserName = model.UserName;
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

        public int FavoriteFoodsCount(string userId)
            => this.usersRepository
            .AllAsNoTracking()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.FavoriteFoods)
            .Count();
    }
}
