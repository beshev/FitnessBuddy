namespace FitnessBuddy.Services.Data.UsersFollowers
{
    using System.Threading.Tasks;

    public interface IUsersFollowersService
    {
        public Task FollowAsync(string userId, string followerId);

        public Task UnFollowAsync(string userId, string followerId);

        public Task<bool> IsFollowingByUserAsync(string userUsername, string followerUsername);
    }
}
