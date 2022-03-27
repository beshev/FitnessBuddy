namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.ExercisesLikes;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ExercisesLikesServiceTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(2)]
        public async Task GetExerciseLikesCountAsyncShouldReturnCountOfAllExercieLikes(int exerciseId)
        {
            var list = new List<ExerciseLike>()
            {
                new ExerciseLike
                {
                    ExerciseId = 1,
                },
                new ExerciseLike
                {
                    ExerciseId = 1,
                },
                new ExerciseLike
                {
                    ExerciseId = 2,
                },
            };

            var mockRepo = MockRepo.MockRepository<ExerciseLike>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesLikesService(mockRepo.Object);

            var actual = await service.GetExerciseLikesCountAsync(exerciseId);
            var expected = list.Where(x => x.ExerciseId == exerciseId).Count();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsExistsShouldReturnTrueIfUserIsAlreadyLikeExercise(int exerciseId, string userId)
        {
            var list = new List<ExerciseLike>()
            {
                new ExerciseLike
                {
                    ExerciseId = 1,
                    UserId = "1",
                },
                new ExerciseLike
                {
                    ExerciseId = 2,
                    UserId = "2",
                },
            };

            var mockRepo = MockRepo.MockRepository<ExerciseLike>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesLikesService(mockRepo.Object);

            var actual = await service.IsExists(userId, exerciseId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, "3")]
        [InlineData(2, "4")]
        public async Task IsExistsShouldReturnFalseIfUserNotLikeExercise(int exerciseId, string userId)
        {
            var list = new List<ExerciseLike>()
            {
                new ExerciseLike
                {
                    ExerciseId = 1,
                    UserId = "1",
                },
                new ExerciseLike
                {
                    ExerciseId = 2,
                    UserId = "2",
                },
            };

            var mockRepo = MockRepo.MockRepository<ExerciseLike>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesLikesService(mockRepo.Object);

            var actual = await service.IsExists(userId, exerciseId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("1", 2)]
        [InlineData("2", 1)]
        public async Task LikeAsyncShouldAddExerciseLike(string userId, int exerciseId)
        {
            var list = new List<ExerciseLike>();

            var mockRepo = MockRepo.MockRepository<ExerciseLike>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<ExerciseLike>()))
                .Callback((ExerciseLike exerciseLike) => list.Add(exerciseLike));

            var service = new ExercisesLikesService(mockRepo.Object);

            await service.LikeAsync(userId, exerciseId);

            var actual = list.FirstOrDefault();

            actual.Should().NotBeNull();
            actual.UserId.Should().Be(userId);
            actual.ExerciseId.Should().Be(exerciseId);
        }

        [Theory]
        [InlineData("1", 2)]
        [InlineData("2", 1)]
        public async Task UnLikeAsyncShouldRemoveExerciseLike(string userId, int exerciseId)
        {
            var list = new List<ExerciseLike>()
            {
                new ExerciseLike
                {
                    UserId = "1",
                    ExerciseId = 2,
                },
                new ExerciseLike
                {
                    UserId = "2",
                    ExerciseId = 1,
                },
            };

            var mockRepo = MockRepo.MockRepository<ExerciseLike>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.Delete(It.IsAny<ExerciseLike>()))
                .Callback((ExerciseLike exerciseLike) => list.Remove(exerciseLike));

            var service = new ExercisesLikesService(mockRepo.Object);

            await service.LikeAsync(userId, exerciseId);

            list.Should().HaveCount(2);
            await service.UnLikeAsync(userId, exerciseId);

            list.Should().HaveCount(1);
        }
    }
}
