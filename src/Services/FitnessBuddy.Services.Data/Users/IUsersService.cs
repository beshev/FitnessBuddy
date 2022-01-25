﻿namespace FitnessBuddy.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FitnessBuddy.Web.ViewModels.Users;

    public interface IUsersService
    {
        public UserViewModel GetUserInfo(string userId);

        public Task AddFoodToFavoriteAsync(string userId, Food food);

        public Task RemoveFoodFromFavoriteAsync(string userId, Food food);

        public IEnumerable<FoodViewModel> GetFavoriteFoods(string userId);

        public bool IsFoodFavorite(string userId, int foodId);

        public bool HasMeal(string userId);

        public Task EditAsync(string userId, UserInputModel model);
    }
}
