namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.MealsFoodsService;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class MealsFoodsServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddFoodToMealIfTheFoodIsNotAdded()
        {
            var list = new List<MealFood>();

            var mockRepo = MockRepo.MockDeletableRepository<MealFood>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<MealFood>())).Callback((MealFood mealFood) => list.Add(mealFood));

            var service = new MealsFoodsService(mockRepo.Object);

            var mealFood = new MealFoodInputModel
            {
                FoodId = 1,
                MealId = 2,
                QuantityInGrams = 3,
            };

            await service.AddAsync(mealFood);

            var actual = list.FirstOrDefault();

            actual.Should().NotBeNull();
            actual.MealId.Should().Be(2);
            actual.FoodId.Should().Be(1);
            actual.QuantityInGrams.Should().Be(3);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ContainsAsyncShouldReturnTrueIfMealFoodExist(int mealFoodId)
        {
            var list = new List<MealFood>()
            {
                new MealFood
                {
                    Id = 1,
                },
                new MealFood
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<MealFood>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsFoodsService(mockRepo.Object);

            var actual = await service.ContainsAsync(mealFoodId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task ContainsAsyncShouldReturnFalseIfMealFoodNotExist(int mealFoodId)
        {
            var list = new List<MealFood>()
            {
                new MealFood
                {
                    Id = 1,
                },
                new MealFood
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<MealFood>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsFoodsService(mockRepo.Object);

            var actual = await service.ContainsAsync(mealFoodId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdAsyncShouldReturnMealFood(int mealFoodId)
        {
            var list = new List<MealFood>()
            {
                new MealFood
                {
                    Id = 1,
                },
                new MealFood
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<MealFood>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsFoodsService(mockRepo.Object);

            var actual = await service.GetByIdAsync(mealFoodId);
            var expected = list.FirstOrDefault(x => x.Id == mealFoodId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsyncShouldSetIsDeletedToTrue(int mealFoodId)
        {
            var list = new List<MealFood>()
            {
                new MealFood
                {
                    Id = 1,
                    IsDeleted = false,
                },
                new MealFood
                {
                    Id = 2,
                    IsDeleted = false,
                },
            };

            var deletedOn = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<MealFood>();
            mockRepo
                .Setup(x => x.Delete(It.IsAny<MealFood>()))
                .Callback((MealFood mealFood) =>
                {
                    mealFood.IsDeleted = true;
                    mealFood.DeletedOn = deletedOn;
                });

            var service = new MealsFoodsService(mockRepo.Object);

            var actual = list.FirstOrDefault(x => x.Id == mealFoodId);

            await service.DeleteAsync(actual);

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(deletedOn);
        }
    }
}
