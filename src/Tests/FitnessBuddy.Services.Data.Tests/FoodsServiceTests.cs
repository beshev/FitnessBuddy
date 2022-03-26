namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class FoodsServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddFood()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>();

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Food>())).Callback((Food food) => list.Add(food));

            var foodNamesList = new List<FoodName>();

            var mockFoodNameRepo = MockRepo.MockDeletableRepository<FoodName>();
            mockFoodNameRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(foodNamesList.AsQueryable().BuildMock());
            mockFoodNameRepo
                .Setup(x => x.AddAsync(It.IsAny<FoodName>()))
                .Callback((FoodName foodName) => foodNamesList.Add(foodName));

            var foodNamesService = new FoodNamesService(mockFoodNameRepo.Object);
            var foodsService = new FoodsService(mockRepo.Object, foodNamesService);

            var food = new FoodInputModel
            {
                Name = "Test food",
                CarbohydratesIn100Grams = 10,
                ProteinIn100Grams = 30.45,
                FatIn100Grams = 2,
                ImageUrl = "some URL",
                Sodium = 0.2,
            };

            var userId = "1";

            await foodsService.AddAsync(userId, food);

            var actual = list.FirstOrDefault();
            var foodName = foodNamesList.FirstOrDefault();

            actual.Should().BeEquivalentTo(food, opt => opt.Excluding(x => x.Name));
            foodName.Name.Should().Be("Test food");
        }

        [Fact]
        public async Task EditAsyncShouldEditFood()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>();

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Food>())).Callback((Food food) => list.Add(food));

            var foodNamesList = new List<FoodName>();

            var mockFoodNameRepo = MockRepo.MockDeletableRepository<FoodName>();
            mockFoodNameRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(foodNamesList.AsQueryable().BuildMock());
            mockFoodNameRepo
                .Setup(x => x.AddAsync(It.IsAny<FoodName>()))
                .Callback((FoodName foodName) => foodNamesList.Add(foodName));

            var foodNamesService = new FoodNamesService(mockFoodNameRepo.Object);
            var foodsService = new FoodsService(mockRepo.Object, foodNamesService);

            var food = new FoodInputModel
            {
                Name = "Test food",
                CarbohydratesIn100Grams = 10,
                ProteinIn100Grams = 30.45,
                FatIn100Grams = 2,
                ImageUrl = "some URL",
                Sodium = 0.2,
            };

            var userId = "1";

            await foodsService.AddAsync(userId, food);

            var editedFood = new FoodInputModel
            {
                Name = "Test food",
                CarbohydratesIn100Grams = 100,
                ProteinIn100Grams = 350.45,
                FatIn100Grams = 20,
                ImageUrl = "some URL",
                Sodium = 0.20,
            };

            await foodsService.EditAsync(editedFood);

            var actual = list.FirstOrDefault();
            var foodName = foodNamesList.FirstOrDefault();

            actual.Should().BeEquivalentTo(editedFood, opt => opt.Excluding(x => x.Name));
            foodName.Name.Should().Be("Test food");
        }

        [Fact]
        public async Task DeleteAsyncShouldSetIsDeletedToTrue()
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodNameId = 1,
                    CarbohydratesIn100Grams = 10,
                    ProteinIn100Grams = 30.45,
                    FatIn100Grams = 2,
                    ImageUrl = "some URL",
                    Sodium = 0.2,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock());
            mockRepo
                .Setup(x => x.Delete(It.IsAny<Food>()))
                .Callback((Food food) =>
                {
                    food.IsDeleted = true;
                    food.DeletedOn = dateTime;
                });

            var foodsService = new FoodsService(mockRepo.Object, null);

            await foodsService.DeleteAsync(2);

            var actual = list.FirstOrDefault();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllFoodsOrderedByCreatedOn()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodNameId = 1,
                    CarbohydratesIn100Grams = 3,
                    ProteinIn100Grams = 21,
                    FatIn100Grams = 22,
                    ImageUrl = "some URL",
                    Sodium = 13,
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    FoodNameId = 1,
                    CarbohydratesIn100Grams = 10,
                    ProteinIn100Grams = 30.45,
                    FatIn100Grams = 2,
                    ImageUrl = "some URL",
                    Sodium = 0.2,
                    CreatedOn = DateTime.Now.AddDays(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetAllAsync<Food>();
            var expected = list.OrderByDescending(x => x.CreatedOn);

            actual.Should().HaveCount(2);
            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task GetAllAsyncShouldReturnAllFoodsAddedByUserOrderedByCreatedOn(string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "1",
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "2",
                    CreatedOn = DateTime.Now.AddDays(2),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetAllAsync<Food>(userId);
            var expected = list
                .Where(x => x.AddedByUserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData("Rice")]
        [InlineData("Bob")]
        [InlineData("food")]
        public async Task GetAllAsyncShouldReturnAllFoodsThatContainsSearchInNamesOrderedByCreatedOn(string search)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodName = new FoodName { Name = "Rice food" },
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "1",
                    FoodName = new FoodName { Name = "Bob food" },
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "2",
                    FoodName = new FoodName { Name = "food" },
                    CreatedOn = DateTime.Now.AddDays(2),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetAllAsync<Food>(null, search);
            var expected = list
                .Where(x => x.FoodName.Name.Contains(search))
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(0, 1)]
        [InlineData(2, 0)]
        public async Task GetAllAsyncShouldSkipAndTakeCountOfAllFoodsOrderedByCreatedOn(int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodName = new FoodName { Name = "Rice food" },
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "1",
                    FoodName = new FoodName { Name = "Bob food" },
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "2",
                    FoodName = new FoodName { Name = "food" },
                    CreatedOn = DateTime.Now.AddYears(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetAllAsync<Food>(null, null, skip, take);
            var expected = list
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take)
                .ToList();

            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData("1", "food", 1, 2)]
        public async Task GetAllAsyncShouldReturnAllFoodsAddedByUserAndContainsSerachInNameAndSkipAndTakeCountOfThemOrderedByCreatedOn(string userId, string search, int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    AddedByUserId = "1",
                    FoodName = new FoodName { Name = "Rice food" },
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "1",
                    FoodName = new FoodName { Name = "Bob food" },
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "1",
                    FoodName = new FoodName { Name = "Some food" },
                    CreatedOn = DateTime.Now.AddYears(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetAllAsync<Food>(null, null, skip, take);
            var expected = list
                .Where(x => x.AddedByUserId == userId)
                .Where(x => x.FoodName.Name.Contains(search))
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take)
                .ToList();

            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdAsyncShouldReturnFood(int foodId)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodNameId = 1,
                    FoodName = new FoodName { Name = "Rice" },
                    CarbohydratesIn100Grams = 3,
                    ProteinIn100Grams = 21,
                    FatIn100Grams = 22,
                    ImageUrl = "some URL",
                    Sodium = 13,
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    FoodNameId = 1,
                    FoodName = new FoodName { Name = "Bob" },
                    CarbohydratesIn100Grams = 10,
                    ProteinIn100Grams = 30.45,
                    FatIn100Grams = 2,
                    ImageUrl = "some URL",
                    Sodium = 0.2,
                    CreatedOn = DateTime.Now.AddDays(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetByIdAsync(foodId);
            var expected = list.FirstOrDefault(x => x.Id == foodId);

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected, opt => opt.IncludingNestedObjects());
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetByIdAsyncShouldReturnNullIfFoodDoesNotExist(int foodId)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                },
                new Food
                {
                    Id = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetByIdAsync(foodId);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdAsNoTrackingAsyncShouldReturnFood(int foodId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodNameId = 1,
                    FoodName = new FoodName { Name = "Rice" },
                    CarbohydratesIn100Grams = 3,
                    ProteinIn100Grams = 21,
                    FatIn100Grams = 22,
                    ImageUrl = "some URL",
                    Sodium = 13,
                    CreatedOn = DateTime.Now,
                },
                new Food
                {
                    Id = 1,
                    FoodNameId = 1,
                    FoodName = new FoodName { Name = "Bob" },
                    CarbohydratesIn100Grams = 10,
                    ProteinIn100Grams = 30.45,
                    FatIn100Grams = 2,
                    ImageUrl = "some URL",
                    Sodium = 0.2,
                    CreatedOn = DateTime.Now.AddDays(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetByIdAsNoTrackingAsync<Food>(foodId);
            var expected = list.FirstOrDefault(x => x.Id == foodId);

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected, opt => opt.IncludingNestedObjects());
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetByIdAsNoTrackingAsyncShouldReturnNullIfFoodDoesNotExist(int foodId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                },
                new Food
                {
                    Id = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetByIdAsNoTrackingAsync<Food>(foodId);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData("1", 2)]
        [InlineData("2", 1)]
        public async Task IsUserFoodAsyncShouldReturnTrueIfFoodIsAddedByUser(string userId, int foodId)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.IsUserFoodAsync(userId, foodId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData("1", 2)]
        [InlineData("2", 1)]
        public async Task IsUserFoodAsyncShouldReturnFalseIfFoodIsNotAddedByUser(string userId, int foodId)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    AddedByUserId = "3",
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "4",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.IsUserFoodAsync(userId, foodId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public async Task IsExistAsyncShouldReturnTrueIfFoodExist(int foodId)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                },
                new Food
                {
                    Id = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.IsExistAsync(foodId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IsExistAsyncShouldReturnFalseIfFoodNotExist(int foodId)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                },
                new Food
                {
                    Id = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.IsExistAsync(foodId);

            actual.Should().BeFalse();
        }

        [Fact]
        public async Task GetCountAsyncShouldReturnAllFoodsCount()
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                },
                new Food
                {
                    Id = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetCountAsync();

            actual.Should().Be(2);
        }

        [Theory]
        [InlineData("1", 2)]
        [InlineData("2", 1)]
        [InlineData("3", 0)]
        public async Task GetCountAsyncShouldReturnAllFoodsCountAddedByUser(string userId, int expected)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    AddedByUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetCountAsync(userId);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Rice", 1)]
        [InlineData("Bob", 1)]
        [InlineData("food", 3)]
        public async Task GetCountAsyncShouldReturnAllFoodsCountThatNamesContainsSearch(string search, int expected)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodName = new FoodName { Name = "Rice food" },
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    FoodName = new FoodName { Name = "Bob food" },
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    FoodName = new FoodName { Name = "food" },
                    AddedByUserId = "2",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetCountAsync(null, search);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("1", "Rice", 1)]
        [InlineData("1", "Bob", 1)]
        [InlineData("1", "food", 3)]
        [InlineData("2", "food", 0)]
        public async Task GetCountAsyncShouldReturnAllFoodsCountThatNamesContainsSearchAndAreAddedByUser(string userId, string search, int expected)
        {
            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodName = new FoodName { Name = "Rice food" },
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    FoodName = new FoodName { Name = "Bob food" },
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    FoodName = new FoodName { Name = "food" },
                    AddedByUserId = "1",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetCountAsync(userId, search);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetRandomAsyncShouldReturnCountOfAllFoods(int count)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Food>()
            {
                new Food
                {
                    Id = 2,
                    FoodName = new FoodName { Name = "Rice food" },
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    FoodName = new FoodName { Name = "Bob food" },
                    AddedByUserId = "1",
                },
                new Food
                {
                    Id = 1,
                    FoodName = new FoodName { Name = "food" },
                    AddedByUserId = "1",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Food>();
            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock());

            var foodsService = new FoodsService(mockRepo.Object, null);

            var actual = await foodsService.GetRandomAsync<Food>(count);
            var expected = list.OrderByDescending(x => Guid.NewGuid()).Take(count);

            actual.Should().HaveCount(count);
        }
    }
}
