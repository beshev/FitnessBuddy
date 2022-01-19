namespace FitnessBuddy.Services.Data.Users
{
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Users;

    public interface IUsersService
    {
        public UserProfileInputModel GetUserInfo(string userId);

        public Task EditAsync(string userId, UserProfileInputModel model);
    }
}
