namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.UsersFollowers;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class UsersFollowersServiceTests
    {
        [Fact]
        public async Task FollowAsyncShouldAddUserFollower()
        {
            var list = new List<UserFollower>();

            var mockRepo = MockRepo.MockRepository<UserFollower>();
            mockRepo
                .Setup(x => x.AddAsync(It.IsAny<UserFollower>()))
                .Callback((UserFollower userFollower) => list.Add(userFollower));

            var service = new UsersFollowersService(mockRepo.Object);

            var userId = "1";
            var followerId = "2";

            await service.FollowAsync(userId, followerId);

            var actual = list.FirstOrDefault();

            actual.Should().NotBeNull();
            actual.UserId.Should().Be(userId);
            actual.FollowerId.Should().Be(followerId);
        }

        [Theory]
        [InlineData("1", "2")]
        [InlineData("2", "1")]
        public async Task IsFollowingByUserAsyncShouldReturnTrueIfUserFollowOtherUser(string userId, string followerId)
        {
            var list = new List<UserFollower>()
            {
                new UserFollower
                {
                    UserId = "1",
                    FollowerId = "2",
                },
                new UserFollower
                {
                    UserId = "2",
                    FollowerId = "1",
                },
            };

            var mockRepo = MockRepo.MockRepository<UserFollower>();
            mockRepo
                .Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersFollowersService(mockRepo.Object);

            var actual = await service.IsFollowingByUserAsync(userId, followerId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData("1", "2")]
        [InlineData("2", "1")]
        public async Task IsFollowingByUserAsyncShouldReturnFalseIfUserNotFollowOtherUser(string userId, string followerId)
        {
            var list = new List<UserFollower>()
            {
                new UserFollower
                {
                    UserId = "1",
                    FollowerId = "3",
                },
                new UserFollower
                {
                    UserId = "2",
                    FollowerId = "4",
                },
            };

            var mockRepo = MockRepo.MockRepository<UserFollower>();
            mockRepo
                .Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersFollowersService(mockRepo.Object);

            var actual = await service.IsFollowingByUserAsync(userId, followerId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("1", "2")]
        [InlineData("2", "1")]
        public async Task UnFollowAsyncShouldRemoveUserFollower(string userId, string followerId)
        {
            var list = new List<UserFollower>()
            {
                new UserFollower
                {
                    UserId = "1",
                    FollowerId = "2",
                },
                new UserFollower
                {
                    UserId = "2",
                    FollowerId = "1",
                },
            };

            var mockRepo = MockRepo.MockRepository<UserFollower>();
            mockRepo
                .Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo
                .Setup(x => x.Delete(It.IsAny<UserFollower>()))
                .Callback((UserFollower userFollower) => list.Remove(userFollower));

            var service = new UsersFollowersService(mockRepo.Object);

            list.Should().HaveCount(2);

            await service.UnFollowAsync(userId, followerId);

            list.Should().HaveCount(1);
        }
    }
}
