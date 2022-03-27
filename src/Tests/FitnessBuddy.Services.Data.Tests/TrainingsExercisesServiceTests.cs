namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Web.ViewModels.Trainings;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class TrainingsExercisesServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddExerciseToTrainingIfExrciseIsNotAlreadyAdded()
        {
            var list = new List<TrainingExercise>();

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AddAsync(It.IsAny<TrainingExercise>()))
                .Callback((TrainingExercise trainingExercise) => list.Add(trainingExercise));
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var trainingExercise = new TrainingExerciseInputModel
            {
                ExerciseId = 1,
                TrainingId = 2,
                Sets = 8,
                Repetitions = 12,
                Weight = 30,
            };

            await service.AddAsync(trainingExercise);

            var actual = list.FirstOrDefault();

            actual.Should().BeEquivalentTo(trainingExercise);
        }

        [Fact]
        public async Task AddAsyncShouldChangePropertiesOfExerciseIfExerciseIsAlreadyInTraining()
        {
            var list = new List<TrainingExercise>();

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AddAsync(It.IsAny<TrainingExercise>()))
                .Callback((TrainingExercise trainingExercise) => list.Add(trainingExercise));
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var trainingExercise = new TrainingExerciseInputModel
            {
                ExerciseId = 1,
                TrainingId = 2,
                Sets = 8,
                Repetitions = 12,
                Weight = 30,
            };

            await service.AddAsync(trainingExercise);

            trainingExercise.Sets = 10;
            trainingExercise.Repetitions = 14;
            trainingExercise.Weight = 40;

            await service.AddAsync(trainingExercise);

            var actual = list.FirstOrDefault();

            actual.Should().BeEquivalentTo(trainingExercise);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetTrainingExercisesAsyncShouldReturnAllTrainingExercises(int trainingId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<TrainingExercise>()
            {
                new TrainingExercise
                {
                    TrainingId = 1,
                },
                new TrainingExercise
                {
                    TrainingId = 1,
                },
                new TrainingExercise
                {
                    TrainingId = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var actual = await service.GetTrainingExercisesAsync<TrainingExercise>(trainingId);
            var expected = list.Where(x => x.TrainingId == trainingId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnTrueIfSuchTrainingExerciseExist(int trainingExerciseId)
        {
            var list = new List<TrainingExercise>()
            {
                new TrainingExercise
                {
                    Id = 1,
                },
                new TrainingExercise
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var actual = await service.IsExistAsync(trainingExerciseId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IsExistAsyncShouldReturnFalseIfSuchTrainingExerciseNotExist(int trainingExerciseId)
        {
            var list = new List<TrainingExercise>()
            {
                new TrainingExercise
                {
                    Id = 1,
                },
                new TrainingExercise
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var actual = await service.IsExistAsync(trainingExerciseId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsForUserAsyncShouldReturnTrueIfTrainingIsForGiveUser(int trainingExerciseId, string userId)
        {
            var list = new List<TrainingExercise>()
            {
                new TrainingExercise
                {
                    Id = 1,
                    Training = new Training { ForUserId = "1" },
                },
                new TrainingExercise
                {
                    Id = 2,
                    Training = new Training { ForUserId = "2" },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var actual = await service.IsForUserAsync(trainingExerciseId, userId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsForUserAsyncShouldReturnFalseIfTrainingIsNotForGiveUser(int trainingExerciseId, string userId)
        {
            var list = new List<TrainingExercise>()
            {
                new TrainingExercise
                {
                    Id = 1,
                    Training = new Training { ForUserId = "3" },
                },
                new TrainingExercise
                {
                    Id = 2,
                    Training = new Training { ForUserId = "4" },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsExercisesService(mockRepo.Object);

            var actual = await service.IsForUserAsync(trainingExerciseId, userId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task RemoveAsyncShouldSetIsDeletedToTrueForGiveTrainingExercise(int trainingExerciseId)
        {
            var list = new List<TrainingExercise>()
            {
                new TrainingExercise
                {
                    Id = 1,
                },
                new TrainingExercise
                {
                    Id = 2,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<TrainingExercise>();
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.Delete(It.IsAny<TrainingExercise>()))
                .Callback((TrainingExercise trainingExercise) =>
                {
                    trainingExercise.IsDeleted = true;
                    trainingExercise.DeletedOn = dateTime;
                });

            var service = new TrainingsExercisesService(mockRepo.Object);

            await service.RemoveAsync(trainingExerciseId);

            var actual = list.FirstOrDefault(x => x.Id == trainingExerciseId);

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }
    }
}
