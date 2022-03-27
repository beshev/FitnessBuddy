namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Trainings;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class TrainingsServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddTrainingForUserIfTrainingIsNotAlreadyAdded()
        {
            var list = new List<Training>();

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Training>())).Callback((Training training) => list.Add(training));

            var service = new TrainingsService(mockRepo.Object);

            await service.AddAsync("Workout", "1");

            var actual = list.FirstOrDefault();

            actual.Name.Should().Be("Workout");
            actual.ForUserId.Should().Be("1");
        }

        [Fact]
        public async Task AddAsyncShouldNotAddTrainingForUserIfTrainingIsAlreadyAdded()
        {
            var list = new List<Training>();

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Training>())).Callback((Training training) => list.Add(training));

            var service = new TrainingsService(mockRepo.Object);

            await service.AddAsync("Workout", "1");

            var actual = list.FirstOrDefault();

            actual.Name.Should().Be("Workout");
            actual.ForUserId.Should().Be("1");

            await service.AddAsync("Workout", "1");

            list.Count.Should().Be(1);
        }

        [Theory]
        [InlineData(4, "1")]
        [InlineData(3, "2")]
        public async Task DeleteAsyncShouldSetIsDeletedToTrueForUserTraining(int trainingId, string userId)
        {
            var list = new List<Training>()
            {
                new Training
                {
                    Id = 4,
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 3,
                    ForUserId = "2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            mockRepo
                .Setup(x => x.Delete(It.IsAny<Training>()))
                .Callback((Training training) =>
                {
                    training.IsDeleted = true;
                    training.DeletedOn = dateTime;
                });

            var service = new TrainingsService(mockRepo.Object);

            await service.DeleteAsync(trainingId, userId);

            var actual = list.FirstOrDefault(x => x.Id == trainingId);

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task GetAllAsyncShouldReturnAllUserTrainings(string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 4,
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 3,
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 1,
                    ForUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.GetAllAsync<Training>(userId);
            var expected = list.Where(x => x.ForUserId == userId).ToList();

            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetNameByIdAsyncShouldReturnTrainingName(int trainingId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 2,
                    ForUserId = "1",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.GetNameByIdAsync(trainingId);
            var expected = list.FirstOrDefault(x => x.Id == trainingId)?.Name;

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Workout", "1")]
        [InlineData("Workout", "2")]
        public async Task GetTrainingIdAsyncReceiveTrainingNameAndUserIdAndShouldReturnTrainingId(string trainingName, string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    Name = "Workout",
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 2,
                    Name = "Workout",
                    ForUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.GetTrainingIdAsync(trainingName, userId);
            var expected = list.FirstOrDefault(x => x.Name == trainingName && x.ForUserId == userId).Id;

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Workout", "1")]
        [InlineData("Workout", "2")]
        public async Task GetTrainingIdAsyncShouldReturnMinusOneIfUserDoesNotHaveSuchTraining(string trainingName, string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    Name = "Training A",
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 2,
                    Name = "Training B",
                    ForUserId = "2",
                },
                new Training
                {
                    Id = 4,
                    Name = "Workout",
                    ForUserId = "3",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.GetTrainingIdAsync(trainingName, userId);

            actual.Should().Be(-1);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnTrueIfTrainingExist(int trainingId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    Name = "Workout",
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 2,
                    Name = "Workout",
                    ForUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.IsExistAsync(trainingId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IsExistAsyncShouldReturnFalseIfTrainingDoesNotExist(int trainingId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    Name = "Workout",
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 2,
                    Name = "Workout",
                    ForUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.IsExistAsync(trainingId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsUserTrainingAsyncShouldReturnTrueIfTrainingIsForUser(int trainingId, string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    Name = "Workout",
                    ForUserId = "1",
                },
                new Training
                {
                    Id = 2,
                    Name = "Workout",
                    ForUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.IsUserTrainingAsync(trainingId, userId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsUserTrainingAsyncShouldReturnFalseIfTrainingIsNotForUser(int trainingId, string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Training>()
            {
                new Training
                {
                    Id = 1,
                    Name = "Workout",
                    ForUserId = "3",
                },
                new Training
                {
                    Id = 2,
                    Name = "Workout",
                    ForUserId = "4",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Training>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new TrainingsService(mockRepo.Object);

            var actual = await service.IsUserTrainingAsync(trainingId, userId);

            actual.Should().BeFalse();
        }
    }
}
