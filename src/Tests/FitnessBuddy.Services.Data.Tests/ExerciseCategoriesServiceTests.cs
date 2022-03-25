namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Exercises;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Xunit;

    public class ExerciseCategoriesServiceTests
    {
        [Fact]
        public async Task GetAllAsyncShouldReturnAllExerciseCategories()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseCategory>()
            {
                new ExerciseCategory
                {
                    Name = "Test 1",
                    Id = 1,
                    Exercises = new List<Exercise>(),
                },
                new ExerciseCategory
                {
                    Name = "Test 2",
                    Id = 2,
                    Exercises = new List<Exercise>(),
                },
                new ExerciseCategory
                {
                    Name = "Test 3",
                    Id = 3,
                    Exercises = new List<Exercise>(),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetAllAsync<ExerciseCategory>();

            actual.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnEmptyCollectionIfThereAreNotCategories()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseCategory>();

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetAllAsync<ExerciseCategory>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCategoryExercisesAsyncShouldReturnAllCategoryExercises()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseCategory>
            {
                new ExerciseCategory
                {
                    Name = "Test category",
                    Id = 1,
                    Exercises = new List<Exercise>
                    {
                        new Exercise()
                        {
                            Name = "exercise test",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 2",
                        },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryExercisesAsync<Exercise>("Test category");

            actual.Should().BeEquivalentTo(list[0].Exercises);
        }

        [Fact]
        public async Task GetCategoryExercisesAsyncShouldReturnAnEmptyCollectionIfCategoryDontHaveExercises()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseCategory>
            {
                new ExerciseCategory
                {
                    Name = "With Exercises",
                    Id = 1,
                    Exercises = new List<Exercise>
                    {
                        new Exercise()
                        {
                            Name = "exercise test",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 2",
                        },
                    },
                },
                new ExerciseCategory
                {
                    Name = "Without Exercises",
                    Id = 2,
                    Exercises = new List<Exercise>(),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryExercisesAsync<Exercise>("Without Exercises");

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCategoryExercisesAsyncShouldSkip1AndTake2Exercises()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseCategory>
            {
                new ExerciseCategory
                {
                    Name = "With Exercises",
                    Id = 1,
                    Exercises = new List<Exercise>
                    {
                        new Exercise()
                        {
                            Name = "exercise test",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 2",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 3",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 4",
                        },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryExercisesAsync<Exercise>("With Exercises", 1, 2);
            var expected = list[0].Exercises.Skip(1).Take(2);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCategoryExercisesCountAsyncShouldReturnExerciseCountOfCategory()
        {
            var list = new List<ExerciseCategory>
            {
                new ExerciseCategory
                {
                    Name = "With Exercises",
                    Id = 1,
                    Exercises = new List<Exercise>
                    {
                        new Exercise()
                        {
                            Name = "exercise test",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 2",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 3",
                        },
                        new Exercise()
                        {
                            Name = "exercise test 4",
                        },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryExercisesCountAsync("With Exercises");

            actual.Should().Be(4);
        }

        [Fact]
        public async Task GetCategoryExercisesCountAsyncShouldReturn0IfCategoryDontHaveExercises()
        {
            var list = new List<ExerciseCategory>
            {
                new ExerciseCategory
                {
                    Name = "Without Exercises",
                    Id = 1,
                    Exercises = new List<Exercise>(),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryExercisesCountAsync("Without Exercises");

            actual.Should().Be(0);
        }
    }
}
