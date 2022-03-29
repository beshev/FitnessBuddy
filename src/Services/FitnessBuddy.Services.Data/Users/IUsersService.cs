namespace FitnessBuddy.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Users;

    public interface IUsersService
    {
        public Task<TModel> GetUserInfoAsync<TModel>(string userId);

        public Task<IEnumerable<TModel>> GetFollowersAsync<TModel>(string userId);

        public Task<IEnumerable<TModel>> GetFollowingAsync<TModel>(string userId);

        public Task<IEnumerable<TModel>> GetAllAsync<TModel>(string username = "", int skip = 0, int? take = null);

        public Task AddFoodToFavoriteAsync(string userId, Food food);

        public Task RemoveFoodFromFavoriteAsync(string userId, Food food);

        public Task<ProfileViewModel> GetProfileDataAsync(string userId);

        public Task<IEnumerable<TModel>> GetFavoriteFoodsAsync<TModel>(string userId, int skip = 0, int? take = null);

        public Task<int> FavoriteFoodsCountAsync(string userId);

        public Task BanUserAsync(string username, string banReason);

        public Task UnbanUserAsync(string username);

        public Task<int> GetCountAsync();

        public Task<bool> IsFoodFavoriteAsync(string userId, int foodId);

        public Task<bool> HasMealAsync(string userId);

        public Task<bool> IsUserBannedAsync(string userId);

        public Task<string> GetIdByUsernameAsync(string username);

        public Task<bool> IsUsernameExistAsync(string username);

        public Task EditAsync(string userId, UserInputModel model);
    }
}
