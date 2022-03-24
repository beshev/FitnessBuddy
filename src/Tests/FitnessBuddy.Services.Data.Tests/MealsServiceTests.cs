namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels;
    using FitnessBuddy.Web.ViewModels.Meals;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class MealsServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldCreateMeal()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var list = new List<Meal>();

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Meal>())).Callback((Meal meal) => list.Add(meal));

            var service = new MealsService(mockRepo.Object);

            var userId = "1";

            var meal = new MealInputModel
            {
                Name = "Test 1",
                TargetCarbs = 230,
                TargetFat = 100,
                TargetProtein = 150,
            };

            var addedMeal = await service.CreateAsync(userId, meal);

            Assert.Single(list);
            Assert.Equal(meal.Name, addedMeal.Name);
            Assert.Equal(meal.TargetCarbs, addedMeal.TargetCarbs);
            Assert.Equal(meal.TargetFat, addedMeal.TargetFat);
            Assert.Equal(meal.TargetProtein, addedMeal.TargetProtein);
            Assert.Equal(userId, addedMeal.ForUserId);
        }

        [Fact]
        public async Task DeleteMethodShouldMarkMealAsDeleted()
        {
            var list = new List<Meal>()
            {
                new Meal
                {
                    Id = 1,
                    Name = "Test",
                },
            };

            var dateTime = DateTime.UtcNow;

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.Delete(It.IsAny<Meal>())).Callback((Meal meal) =>
            {
                meal.IsDeleted = true;
                meal.DeletedOn = dateTime;
            });

            var service = new MealsService(mockRepo.Object);

            var meal = await service.DeleteAsync(1);

            Assert.True(meal.IsDeleted);
            Assert.Equal(dateTime, meal.DeletedOn);
        }

        [Fact]
        public void IsExistMethodShouldReturnFalseIfMealNotExist()
        {
            var list = new List<Meal>()
            {
                new Meal { Id = 2, Name = "Test" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());

            var service = new MealsService(mockRepo.Object);

            Assert.False(service.IsExist(1));
        }

        [Fact]
        public void IsExistMethodShouldReturnTrueIfMealExist()
        {
            var list = new List<Meal>()
            {
                new Meal { Id = 2, Name = "Test" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());

            var service = new MealsService(mockRepo.Object);

            Assert.True(service.IsExist(2));
        }

        [Fact]
        public void GetByIdMethodShouldReturnExistingMeal()
        {
            var list = new List<Meal>()
            {
                new Meal { Name = "Test", Id = 1 },
                new Meal { Name = "Test 2", Id = 2 },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());

            var service = new MealsService(mockRepo.Object);

            var actualMeal = service.GetById(2);

            Assert.Equal("Test 2", actualMeal.Name);
            Assert.Equal(2, actualMeal.Id);
        }

        [Fact]
        public void GetByIdMethodShouldReturnNullIfMealNotExist()
        {
            var list = new List<Meal>()
            {
                new Meal { Name = "Test", Id = 1 },
                new Meal { Name = "Test 2", Id = 2 },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());

            var service = new MealsService(mockRepo.Object);

            var actualMeal = service.GetById(3);

            Assert.Null(actualMeal);
        }

        [Fact]
        public async Task GetUserMealsMethodShouldReturnEmptyCollectionIfUserDontHaveMeals()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test", Id = 2, ForUserId = "1" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var userId = "2";

            var actualList = await service.GetUserMeals<MealViewModel>(userId);

            Assert.Empty(actualList);
        }

        [Fact]
        public async Task GetUserMealsMethodShouldReturnUserMeals()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test 2", Id = 2, ForUserId = "1" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var userId = "1";

            var actualList = (await service.GetUserMeals<MealViewModel>(userId)).ToList();

            Assert.Equal(2, actualList.Count);
            Assert.Equal("Test", actualList[0].Name);
            Assert.Equal("Test 2", actualList[1].Name);
        }

        [Fact]
        public void IsUserMealReturnFalseIfMealIsNotForUser()
        {
            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test 2", Id = 2, ForUserId = "1" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());

            var service = new MealsService(mockRepo.Object);

            var userId = "2";

            Assert.False(service.IsUserMeal(1, userId));
            Assert.False(service.IsUserMeal(2, userId));
        }

        [Fact]
        public void IsUserMealReturnTrueIfMealIsForUser()
        {
            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test 2", Id = 2, ForUserId = "1" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());

            var service = new MealsService(mockRepo.Object);

            var userId = "1";

            Assert.True(service.IsUserMeal(1, userId));
            Assert.True(service.IsUserMeal(2, userId));
        }
    }
}
