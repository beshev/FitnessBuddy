namespace FitnessBuddy.Services.Data.UsersFollowers
{
    using System.Threading.Tasks;

    public interface IUsersFollowersService
    {
        public Task FollowAsync(string userId, string followerId);

        public Task UnFollowAsync(string userId, string followerId);

        public bool IsFollowingByUser(string userUsername, string followerUsername);
    }
}
