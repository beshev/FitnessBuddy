namespace FitnessBuddy.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FitnessBuddy.Web.ViewModels.Users;

    public interface IUsersService
    {
        public TModel GetUserInfo<TModel>(string userId);

        public IEnumerable<TModel> GetFollowers<TModel>(string userId);

        public IEnumerable<TModel> GetFollowing<TModel>(string userId);

        public IEnumerable<TModel> GetAll<TModel>(string username = "", int skip = 0, int? take = null);

        public Task AddFoodToFavoriteAsync(string userId, Food food);

        public Task RemoveFoodFromFavoriteAsync(string userId, Food food);

        public ProfileViewModel GetProfileData(string userId);

        public IEnumerable<FoodViewModel> GetFavoriteFoods(string userId, int skip = 0, int? take = null);

        public int FavoriteFoodsCount(string userId);

        public Task BanUserAsync(string username, string banReason);

        public Task UnbanUserAsync(string username);

        public int GetCount();

        public bool IsFoodFavorite(string userId, int foodId);

        public bool HasMeal(string userId);

        public bool IsUserBanned(string userId);

        public string GetIdByUsername(string username);

        public string GetUsernameById(string userId);

        public bool IsUsernameExist(string username);

        public Task EditAsync(string userId, UserInputModel model, string picturePath);
    }
}
