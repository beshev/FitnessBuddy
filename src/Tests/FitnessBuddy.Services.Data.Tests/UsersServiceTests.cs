namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;
    using FitnessBuddy.Services.Cloudinary;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.ViewModels.Users;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class UsersServiceTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task AddFoodToFavoriteAsyncShouldAddFoodToUserFavoriteFoods(string userId)
        {
            var list = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "Tester",
                    FavoriteFoods = new List<Food>(),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var food = new Food
            {
                Id = 1,
                FoodNameId = 1,
            };

            await service.AddFoodToFavoriteAsync(userId, food);

            var actual = list[0].FavoriteFoods;

            actual.Should().HaveCount(1);
            actual.FirstOrDefault().Should().BeEquivalentTo(food);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("some GUID")]
        public async Task RemoveFoodToFavoriteAsyncShouldRemoveFoodFromUserFavoriteFoods(string userId)
        {
            var food = new Food { Id = 1, FoodNameId = 1 };

            var list = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "Tester",
                    FavoriteFoods = new List<Food>()
                    {
                        new Food { Id = 3, FoodNameId = 2 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            await service.AddFoodToFavoriteAsync(userId, food);
            await service.RemoveFoodFromFavoriteAsync(userId, food);

            var actual = list[0].FavoriteFoods;

            actual.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("some GUID")]
        public async Task EditAsyncShouldEditUserDataWithProfilePicture(string userId)
        {
            var list = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "Tester",
                    AboutMe = "Test me",
                    DailyProteinGoal = 1,
                    DailyCarbohydratesGoal = 1,
                    DailyFatGoal = 1,
                    HeightInCm = 120,
                    WeightInKg = 20,
                    GoalWeightInKg = 30,
                    Gender = GenderType.Male,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(x => x.FileName).Returns("test.txt");

            var mockCloudinaryService = new Mock<ICloudinaryService>();
            mockCloudinaryService
                .Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(Task.Run(() => "ImageUrl"));

            var service = new UsersService(mockRepo.Object, null, mockCloudinaryService.Object);

            var expected = new UserInputModel
            {
                Username = "Tester Edited!",
                AboutMe = "Test me EDITED",
                DailyProteinGoal = 3,
                DailyCarbohydratesGoal = 6,
                DailyFatGoal = 9,
                HeightInCm = 150,
                WeightInKg = 50,
                GoalWeightInKg = 80,
                Gender = GenderType.Female,
                ProfilePicture = mockFormFile.Object,
            };

            await service.EditAsync(userId, expected);

            var actual = list[0];

            actual.Should().BeEquivalentTo(expected, opt =>
            {
                opt.Excluding(x => x.ProfilePicture);
                opt.Excluding(x => x.Username);

                return opt;
            });
            actual.ProfilePicture.Should().Be($"ImageUrl");
            actual.UserName.Should().Be(expected.Username);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("GUID")]
        public async Task GetFavoriteFoodsAsyncShouldReturnAllUserFavoriteFoods(string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "Tester",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1, FoodNameId = 3 },
                        new Food { Id = 2, FoodNameId = 2 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetFavoriteFoodsAsync<Food>(userId);
            var expected = list[0].FavoriteFoods;


            actual.Should().HaveCount(2);
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("GUID", 1, 2)]
        [InlineData("GUID", 0, 2)]
        [InlineData("GUID", 2, 0)]
        [InlineData("No user", 0, 0)]
        public async Task GetFavoriteFoodsAsyncShouldSkipAndTakeCountOfUserFavoriteFoods(string userId, int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "GUID",
                    UserName = "Tester",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1, FoodNameId = 3 },
                        new Food { Id = 2, FoodNameId = 2 },
                        new Food { Id = 3, FoodNameId = 2 },
                        new Food { Id = 4, FoodNameId = 2 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetFavoriteFoodsAsync<Food>(userId, skip, take);
            var expected = list[0].FavoriteFoods
                .Skip(skip)
                .Take(take);


            actual.Should().HaveCount(take);
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task GetUserInfoAsyncShouldReturnUser(string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Email = "Test@mail.com",
                    DailyCarbohydratesGoal = 1,
                    DailyFatGoal = 2,
                    DailyProteinGoal = 3,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                    DailyCarbohydratesGoal = 10,
                    DailyFatGoal = 20,
                    DailyProteinGoal = 30,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetUserInfoAsync<ApplicationUser>(userId);
            var expected = list.FirstOrDefault(x => x.Id == userId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("3")]
        [InlineData("4")]
        public async Task GetUserInfoAsyncShouldReturnNullIfUserNotExist(string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Email = "Test@mail.com",
                    DailyCarbohydratesGoal = 1,
                    DailyFatGoal = 2,
                    DailyProteinGoal = 3,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                    DailyCarbohydratesGoal = 10,
                    DailyFatGoal = 20,
                    DailyProteinGoal = 30,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetUserInfoAsync<ApplicationUser>(userId);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public async Task IsFoodFavoriteAsyncSholudReturnTrueIfFoodIsFavorite(string userId, int foodId)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1 },
                        new Food { Id = 2 },
                    },
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1 },
                        new Food { Id = 2 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.IsFoodFavoriteAsync(userId, foodId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData("1", 4)]
        [InlineData("2", 3)]
        public async Task IsFoodFavoriteAsyncSholudReturnFalseIfFoodIsNotFavorite(string userId, int foodId)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1 },
                        new Food { Id = 2 },
                    },
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1 },
                        new Food { Id = 2 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.IsFoodFavoriteAsync(userId, foodId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task HasMealAsyncShouldReturnTrueIfUserHasMeal(string userId)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Meals = new List<Meal>
                    {
                        new Meal { Id = 1 },
                    },
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                    Meals = new List<Meal>
                    {
                        new Meal { Id = 1 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.HasMealAsync(userId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task HasMealAsyncShouldReturnFalseIfUserDoesNotHaveMeal(string userId)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Meals = new List<Meal>(),
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.HasMealAsync(userId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test 2")]
        public async Task IsUsernameExistAsyncShouldReturnTrueIfUsernameIsAlreadyUsed(string username)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Email = "Test@mail.com",
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.IsUsernameExistAsync(username);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData("Fake username")]
        [InlineData("Some username")]
        public async Task IsUsernameExistAsyncShouldReturnFalseIfUsernameIsNotUsed(string username)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Email = "Test@mail.com",
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.IsUsernameExistAsync(username);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("1", 2)]
        [InlineData("2", 0)]
        [InlineData("3", 0)]
        public async Task FavoriteFoodsCountAsyncShouldReturnUserFavoriteFoodsCount(string userId, int expected)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    FavoriteFoods = new List<Food>
                    {
                        new Food { Id = 1 },
                        new Food { Id = 2 },
                    },
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Test 2",
                    Email = "Test2@mail.com",
                    FavoriteFoods = new List<Food>(),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.FavoriteFoodsCountAsync(userId);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("1")]
        public async Task GetProfileDataAsyncShouldReturnUserProfileData(string userId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Test",
                    Email = "Test@test.com",
                    WeightInKg = 10,
                    GoalWeightInKg = 20,
                    ProfilePicture = "some URL",
                    HeightInCm = 120,
                    DailyProteinGoal = 1,
                    DailyCarbohydratesGoal = 12,
                    DailyFatGoal = 20,
                    AboutMe = "about test",
                    Gender = GenderType.Male,
                    Roles = new List<IdentityUserRole<string>> { new IdentityUserRole<string> { RoleId = "RoleId" } },
                    Followers = new List<UserFollower>(),
                    Following = new List<UserFollower>(),
                },
            };

            var meals = new List<Meal>()
            {
                new Meal
                {
                    ForUserId = "1",
                    TargetCarbs = 2,
                    TargetFat = 3,
                    TargetProtein = 4,
                    MealFoods = new List<MealFood>
                    {
                        new MealFood
                        {
                            Food = new Food
                            {
                                Id = 1,
                                FoodName = new FoodName { Name = "Some food" },
                                ProteinIn100Grams = 20,
                                FatIn100Grams = 30,
                                CarbohydratesIn100Grams = 40,
                            },
                            QuantityInGrams = 200,
                        },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var mockMealRepo = MockRepo.MockDeletableRepository<Meal>();
            mockMealRepo.Setup(x => x.AllAsNoTracking()).Returns(meals.AsQueryable().BuildMock());

            var mealsService = new MealsService(mockMealRepo.Object);
            var usersService = new UsersService(mockRepo.Object, mealsService, null);

            var expected = new ProfileViewModel
            {
                CurrentFats = 60,
                CurrentProtein = 40,
                CurrentCarbohydrates = 80,
                CurrentCalories = 1020,
                UserInfo = new UserViewModel
                {
                    UserName = "Test",
                    Email = "Test@test.com",
                    WeightInKg = 10,
                    GoalWeightInKg = 20,
                    ProfilePicture = "some URL",
                    HeightInCm = 120,
                    DailyProteinGoal = 1,
                    DailyCarbohydratesGoal = 12,
                    DailyFatGoal = 20,
                    AboutMe = "about test",
                    Gender = GenderType.Male.ToString(),
                    FollowersCount = 0,
                    FollowingCount = 0,
                },
            };

            var actual = await usersService.GetProfileDataAsync(userId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("test user", "1")]
        [InlineData("test user 2", "2")]
        public async Task GetIdByUsernameAsyncShouldReturnUserIdIfUsernameExist(string username, string expected)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    DailyCarbohydratesGoal = 1,
                    DailyFatGoal = 2,
                    DailyProteinGoal = 3,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "test user 2",
                    Email = "Test2@mail.com",
                    DailyCarbohydratesGoal = 10,
                    DailyFatGoal = 20,
                    DailyProteinGoal = 30,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetIdByUsernameAsync(username);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test 2")]
        public async Task GetIdByUsernameAsyncShouldReturnNullIfUsernameDoesNotExist(string username)
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    DailyCarbohydratesGoal = 1,
                    DailyFatGoal = 2,
                    DailyProteinGoal = 3,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "test user 2",
                    Email = "Test2@mail.com",
                    DailyCarbohydratesGoal = 10,
                    DailyFatGoal = 20,
                    DailyProteinGoal = 30,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetIdByUsernameAsync(username);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllUsersInDescendingOrderByDateOfRegistration()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    CreatedOn = DateTime.Now,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "test user 2",
                    Email = "Test2@mail.com",
                    DailyCarbohydratesGoal = 10,
                    DailyFatGoal = 20,
                    DailyProteinGoal = 30,
                    CreatedOn = DateTime.Now.AddDays(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetAllAsync<ApplicationUser>();
            var expected = list.OrderByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task GetAllAsyncShouldFindUserBySearchedUsername()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    CreatedOn = DateTime.Now,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "test user 2",
                    Email = "Test2@mail.com",
                    DailyCarbohydratesGoal = 10,
                    DailyFatGoal = 20,
                    DailyProteinGoal = 30,
                    CreatedOn = DateTime.Now.AddDays(1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = (await service.GetAllAsync<ApplicationUser>("test user")).FirstOrDefault();
            var expected = list[0];

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public async Task GetAllAsyncShouldSkipAndTakeCountOfAllUsersInDescendingOrderByRegistrationDate(int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    CreatedOn = DateTime.Now,
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "test user 2",
                    Email = "Test2@mail.com",
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new ApplicationUser
                {
                    Id = "3",
                    UserName = "test user 2",
                    Email = "Test2@mail.com",
                    CreatedOn = DateTime.Now.AddDays(2),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetAllAsync<ApplicationUser>(null, skip, take);
            var expected = list
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task GetFollowersAsyncShouldReturnAllUserFollowers()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    Followers = new List<UserFollower>
                    {
                        new UserFollower
                        {
                            UserId = "1",
                            FollowerId = "2",
                        },
                        new UserFollower
                        {
                            UserId = "1",
                            FollowerId = "3",
                        },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetFollowersAsync<UserFollower>("1");
            var expected = list[0].Followers;

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetFollowingAsyncShouldReturnAllPeopleWhoUserFollow()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    Following = new List<UserFollower>
                    {
                        new UserFollower
                        {
                            UserId = "2",
                            FollowerId = "1",
                        },
                        new UserFollower
                        {
                            UserId = "3",
                            FollowerId = "1",
                        },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetFollowingAsync<UserFollower>("1");
            var expected = list[0].Following;

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountAsyncShouldReturnAllUsers()
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "test user1",
                    Email = "Test@mail1.com",
                },
                new ApplicationUser
                {
                    Id = "3",
                    UserName = "test user2",
                    Email = "Test@mail2.com",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var actual = await service.GetCountAsync();

            actual.Should().Be(3);
        }

        [Fact]
        public async Task BanUserAsyncShouldSetUserIsBannedToTrueAndBanReason()
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var banReason = "for test";

            await service.BanUserAsync("test user", banReason);

            var actual = list[0];

            actual.IsBanned.Should().BeTrue();
            actual.BanReason.Should().Be(banReason);
            actual.BannedOn.Should().NotBeNull();
        }

        [Fact]
        public async Task UnbanUserAsyncShouldSetUserIsBannedToFalseAndBanReasonToEpmtyString()
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new UsersService(mockRepo.Object, null, null);

            var banReason = "for test";
            var username = "test user";

            await service.BanUserAsync(username, banReason);

            await service.UnbanUserAsync(username);

            var actual = list[0];

            actual.IsBanned.Should().BeFalse();
            actual.BanReason.Should().Be(string.Empty);
            actual.BannedOn.Should().BeNull();
        }

        [Fact]
        public async Task IsUserBannedAsyncShouldReturnTrueIfUserIsBanned()
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    IsBanned = false,
                },
            }.AsQueryable().BuildMock();

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list);
            mockRepo.Setup(x => x.All()).Returns(list);

            var service = new UsersService(mockRepo.Object, null, null);

            var banReason = "for test";
            var username = "test user";

            await service.BanUserAsync(username, banReason);

            var actual = await service.IsUserBannedAsync("1");

            actual.Should().BeTrue();
        }

        [Fact]
        public async Task IsUserBannedAsyncShouldReturnFalseIfUserIsNotBanned()
        {
            var list = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "test user",
                    Email = "Test@mail.com",
                    IsBanned = false,
                },
            }.AsQueryable().BuildMock();

            var mockRepo = MockRepo.MockDeletableRepository<ApplicationUser>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list);
            mockRepo.Setup(x => x.All()).Returns(list);

            var service = new UsersService(mockRepo.Object, null, null);

            var banReason = "for test";
            var username = "test user";

            await service.BanUserAsync(username, banReason);
            await service.UnbanUserAsync(username);

            var actual = await service.IsUserBannedAsync("1");

            actual.Should().BeFalse();
        }
    }
}
