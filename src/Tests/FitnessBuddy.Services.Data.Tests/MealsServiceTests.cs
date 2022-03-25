﻿namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using AutoMapper;
    using FitnessBuddy.Data.Common.Models;
    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class MealsServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldCreateMeal()
        {
            this.InitializeAutoMapper<MealInputModel, Meal>();

            var list = new List<Meal>();

            var mockRepo = this.MockDeletableRepository<Meal>();
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

            list.Should().HaveCount(1);
            addedMeal.Should().BeEquivalentTo(meal);
            addedMeal.ForUserId.Should().Be(userId);
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

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.Delete(It.IsAny<Meal>())).Callback((Meal meal) =>
            {
                meal.IsDeleted = true;
                meal.DeletedOn = dateTime;
            });

            var service = new MealsService(mockRepo.Object);

            var meal = await service.DeleteAsync(1);

            meal.IsDeleted.Should().BeTrue();
            meal.DeletedOn.Should().Be(dateTime);
        }

        [Fact]
        public async void IsExistMethodShouldReturnFalseIfMealNotExist()
        {
            var list = new List<Meal>()
            {
                new Meal { Id = 2, Name = "Test" },
            };

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            (await service.IsExistAsync(1)).Should().BeFalse();
        }

        [Fact]
        public async void IsExistMethodShouldReturnTrueIfMealExist()
        {
            var list = new List<Meal>()
            {
                new Meal { Id = 2, Name = "Test" },
            };

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            Assert.True(await service.IsExistAsync(2));
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnExistingMeal()
        {
            var list = new List<Meal>()
            {
                new Meal { Name = "Test", Id = 1 },
                new Meal { Name = "Test 2", Id = 2 },
            };

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var actual = await service.GetByIdAsync(2);

            actual.Name.Should().Be("Test 2");
            actual.Id.Should().Be(2);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullIfMealNotExist()
        {
            var list = new List<Meal>()
            {
                new Meal { Name = "Test", Id = 1 },
                new Meal { Name = "Test 2", Id = 2 },
            };

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var actualMeal = await service.GetByIdAsync(3);

            actualMeal.Should().BeNull();
        }

        [Fact]
        public async Task GetUserMealsMethodShouldReturnEmptyCollectionIfUserDontHaveMeals()
        {
            this.InitializeAutoMapper<Meal>();

            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test", Id = 2, ForUserId = "1" },
            };

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var userId = "2";

            var actualList = await service.GetUserMealsAsync<Meal>(userId);

            actualList.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserMealsMethodShouldReturnUserMeals()
        {
            this.InitializeAutoMapper<Meal>();

            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test 2", Id = 2, ForUserId = "1" },
            };

            var mockRepo = this.MockDeletableRepository<Meal>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var userId = "1";

            var actualList = (await service.GetUserMealsAsync<Meal>(userId)).ToList();

            actualList.Should().HaveCount(2);
            actualList.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async void IsUserMealReturnFalseIfMealIsNotForUser()
        {
            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test 2", Id = 2, ForUserId = "1" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var userId = "2";

            (await service.IsUserMealAsync(1, userId)).Should().BeFalse();
            (await service.IsUserMealAsync(21, userId)).Should().BeFalse();
        }

        [Fact]
        public async void IsUserMealReturnTrueIfMealIsForUser()
        {
            var list = new List<Meal>
            {
                 new Meal { Name = "Test", Id = 1, ForUserId = "1" },
                 new Meal { Name = "Test 2", Id = 2, ForUserId = "1" },
            };

            var mockRepo = new Mock<IDeletableEntityRepository<Meal>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MealsService(mockRepo.Object);

            var userId = "1";

            (await service.IsUserMealAsync(1, userId)).Should().BeTrue();
            (await service.IsUserMealAsync(2, userId)).Should().BeTrue();
        }

        private void InitializeAutoMapper<TSource>()
            where TSource : class
        {
            var config = new MapperConfigurationExpression();
            config.CreateMap<TSource, TSource>();

            AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config));
        }

        private void InitializeAutoMapper<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            var config = new MapperConfigurationExpression();
            config.CreateMap<TSource, TDestination>();

            AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config));
        }

        private Mock<IDeletableEntityRepository<TEntity>> MockDeletableRepository<TEntity>()
           where TEntity : class, IDeletableEntity
        {
            return new Mock<IDeletableEntityRepository<TEntity>>();
        }
    }
}
