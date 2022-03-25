namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ExercisesServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddExercise()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>();

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Exercise>())).Callback((Exercise exercise) => list.Add(exercise));

            var service = new ExercisesService(mockRepo.Object);

            var exercise = new ExerciseInputModel
            {
                Name = "Test",
                Description = "Test description",
                VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                ImageUrl = "Some URL",
                CategoryId = 1,
                Difficulty = ExerciseDifficulty.Beginner,
                EquipmentId = 1,
            };

            var userId = "1";

            await service.AddAsync(userId, exercise);

            list[0].Should().BeEquivalentTo(exercise, opt => opt.Excluding(x => x.VideoUrl));
            list[0].VideoUrl.Should().Be("https://youtube.com/embed/9ZvDBSQa_so");
            list[0].AddedByUserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllExercisesOrderedDescendingByLikesAndCreatedOn()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Test",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "Test",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now.AddDays(2),
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "Test",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Exercise>())).Callback((Exercise exercise) => list.Add(exercise));
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetAllAsync<Exercise>();
            var expected = list
                .OrderByDescending(x => x.ExerciseLikes.Count)
                .ThenByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task GetAllAsyncShouldSkipAndTakeCountExercisesOrderedDescendingByLikesAndCreatedOn(int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Test",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "Test",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now.AddDays(2),
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "Test",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Exercise>())).Callback((Exercise exercise) => list.Add(exercise));
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetAllAsync<Exercise>(null, skip, take);
            var expected = list
                .OrderByDescending(x => x.ExerciseLikes.Count)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("For")]
        [InlineData("exercise")]
        [InlineData("without match")]
        public async Task GetAllAsyncShouldTakeAllExercisesThatContainsSearchInNamesOrderedDescendingByLikesAndCreatedOn(string search)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Test exercise",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "For exercise",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now.AddDays(2),
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Exercise>())).Callback((Exercise exercise) => list.Add(exercise));
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetAllAsync<Exercise>(search);
            var expected = list
                .Where(x => x.Name.Contains(search))
                .OrderByDescending(x => x.ExerciseLikes.Count)
                .ThenByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData("Test", 1, 1)]
        [InlineData("For", 1, 2)]
        [InlineData("exercise", 2, 1)]
        [InlineData("without match", 0, 0)]
        public async Task GetAllAsyncShouldTakeAllExercisesThatContainsSearchInNamesAndSkipAndTakeCountOrderedDescendingByLikesAndCreatedOn(string search, int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Test exercise",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "For exercise",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now.AddDays(2),
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                        new ExerciseLike(),
                    },
                },
                new Exercise
                {
                    Name = "Push-ups",
                    Description = "Test description",
                    VideoUrl = "https://www.youtube.com/watch?v=9ZvDBSQa_so",
                    ImageUrl = "Some URL",
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    EquipmentId = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now.AddDays(2),
                    ExerciseLikes = new List<ExerciseLike>
                    {
                        new ExerciseLike(),
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Exercise>())).Callback((Exercise exercise) => list.Add(exercise));
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetAllAsync<Exercise>(search, skip, take);
            var expected = list
                .Where(x => x.Name.Contains(search))
                .OrderByDescending(x => x.ExerciseLikes.Count)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdAsyncShouldReturnExercise(int id)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Test",
                    Id = id,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetByIdAsync<Exercise>(id);

            actual.Should().BeEquivalentTo(list[0]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdAsyncShouldReturnNullIfExerciseNotExist(int id)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Name = "Test",
                    Id = 3,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetByIdAsync<Exercise>(id);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetCountAsyncShouldReturnExercisesCount()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Id = 3,
                },
                new Exercise
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetCountAsync();

            actual.Should().Be(2);
        }

        [Theory]
        [InlineData("test", 1)]
        [InlineData("for", 1)]
        [InlineData("exrcise", 3)]
        public async Task GetCountAsyncShouldReturnExercisesCountThatContainsSearchInNames(string search, int expextedCount)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Id = 3,
                    Name = "test exrcise",
                },
                new Exercise
                {
                    Id = 1,
                    Name = "for exrcise",
                },
                new Exercise
                {
                    Id = 2,
                    Name = "exrcise",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetCountAsync(search);

            actual.Should().Be(expextedCount);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsUserCreatorAsyncShouldReturnTrueIfUserIsCreatorOfExerice(int exerciseId, string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Id = 1,
                    AddedByUserId = userId,
                },
                new Exercise
                {
                    Id = 2,
                    AddedByUserId = userId,
                },
                new Exercise
                {
                    Id = 3,
                    AddedByUserId = "3",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.IsUserCreatorAsync(userId, exerciseId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(3, "2")]
        public async Task IsUserCreatorAsyncShouldReturnFalseIfUserIsNotCreatorOfExerice(int exerciseId, string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>()
            {
                new Exercise
                {
                    Id = 1,
                    AddedByUserId = "3",
                },
                new Exercise
                {
                    Id = 2,
                    AddedByUserId = userId,
                },
                new Exercise
                {
                    Id = 3,
                    AddedByUserId = "3",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.IsUserCreatorAsync(userId, exerciseId);

            actual.Should().BeFalse();
        }

        [Fact]
        public async Task EditAsyncShouldEditExercise()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Test 2",
                    EquipmentId = 1,
                    CategoryId = 1,
                    Difficulty = ExerciseDifficulty.Beginner,
                    VideoUrl = "https://youtube.com/embed/9ZvDBSQa_so",
                    ImageUrl = "image URL",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.Update(It.IsAny<Exercise>())).Callback((Exercise exercise) => exercise.ModifiedOn = dateTime);

            var service = new ExercisesService(mockRepo.Object);

            var expected = new ExerciseInputModel
            {
                Id = 1,
                Name = "Test edited",
                Description = "Test 2 edited",
                EquipmentId = 2,
                CategoryId = 3,
                Difficulty = ExerciseDifficulty.Expert,
                VideoUrl = "invalid URl",
                ImageUrl = "image URL edited",
            };

            await service.EditAsync(expected);

            var actual = list[0];

            actual.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.VideoUrl));
            actual.VideoUrl.Should().Be("https://youtube.com/embed/");
            actual.ModifiedOn.Should().Be(dateTime);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public async Task DeleteAsyncShouldSetIsDeletedToTrue(int exerciseId)
        {
            var list = new List<Exercise>
            {
                new Exercise
                {
                    Id = exerciseId,
                    Name = "Test",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.Delete(It.IsAny<Exercise>())).Callback((Exercise exercise) =>
            {
                exercise.DeletedOn = dateTime;
                exercise.IsDeleted = true;
            });

            var service = new ExercisesService(mockRepo.Object);

            await service.DeleteAsync(exerciseId);

            var actual = list[0];

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnTrueIfExerciseExist(int exerciseId)
        {
            var list = new List<Exercise>
            {
                new Exercise
                {
                    Id = exerciseId,
                    Name = "Test",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.IsExistAsync(exerciseId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnFalseIfExerciseNotExist(int exerciseId)
        {
            var list = new List<Exercise>
            {
                new Exercise
                {
                    Id = 3,
                    Name = "Test",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.IsExistAsync(exerciseId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetRandomAsyncShouldReturnRandomCollectionWithCountExercises(int count)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Exercise>
            {
                new Exercise
                {
                    Id = 3,
                    Name = "Test",
                },
                new Exercise
                {
                    Id = 1,
                    Name = "Test 2",
                },
                new Exercise
                {
                    Id = 2,
                    Name = "Test 3",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Exercise>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExercisesService(mockRepo.Object);

            var actual = await service.GetRandomAsync<Exercise>(count);

            actual.Should().HaveCount(count);
        }
    }
}
