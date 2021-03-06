namespace FitnessBuddy.Services.Data.UsersFollowers
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class UsersFollowersService : IUsersFollowersService
    {
        private readonly IRepository<UserFollower> usersFollowerRepository;

        public UsersFollowersService(IRepository<UserFollower> usersFollowerRepository)
        {
            this.usersFollowerRepository = usersFollowerRepository;
        }

        public async Task FollowAsync(string userId, string followerId)
        {
            var userFollower = new UserFollower
            {
                UserId = userId,
                FollowerId = followerId,
            };

            await this.usersFollowerRepository.AddAsync(userFollower);
            await this.usersFollowerRepository.SaveChangesAsync();
        }

        public async Task<bool> IsFollowingByUserAsync(string userId, string followerId)
            => await this.usersFollowerRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.FollowerId == followerId);

        public async Task UnFollowAsync(string userId, string followerId)
        {
            var usersFollower = this.usersFollowerRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.FollowerId == followerId);

            this.usersFollowerRepository.Delete(usersFollower);
            await this.usersFollowerRepository.SaveChangesAsync();
        }
    }
}
