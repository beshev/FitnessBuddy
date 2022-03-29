namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Cloudinary;
    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Web.ViewModels.Articles;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ArticlesServiceTests
    {
        [Fact]
        public async Task CreateAsyncShouldAddArtricle()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Article>();

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo
                .Setup(x => x.AddAsync(It.IsAny<Article>()))
                .Callback((Article artile) => list.Add(artile));

            var mockCloudinaryService = new Mock<ICloudinaryService>();
            mockCloudinaryService
                .Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(Task.Run(() => "ImageUrl"));

            var service = new ArticlesService(mockRepo.Object, mockCloudinaryService.Object);

            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.Setup(_ => _.FileName).Returns("Mockfile.txt");

            var expected = new ArticleInputModel
            {
                Title = "Test",
                Content = "Test content",
                CategoryId = 1,
                CreatorId = "1",
                Picture = mockIFormFile.Object,
                Id = 1,
            };

            await service.CreateAsync(expected);

            list.Should().HaveCount(1);
            list[0].Should().BeEquivalentTo(expected, opt => opt.Excluding(prop => prop.Picture));
            list[0].ImageUrl.Should().Be("ImageUrl");
        }

        [Fact]
        public async Task DeleteAsyncShouldSetIsDeletedToTrue()
        {
            var list = new List<Article>()
            {
                new Article() { Id = 1 },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.Delete(It.IsAny<Article>())).Callback((Article article) =>
            {
                article.IsDeleted = true;
                article.DeletedOn = dateTime;
            });

            var service = new ArticlesService(mockRepo.Object, null);

            await service.DeleteAsync(1);

            list[0].IsDeleted.Should().BeTrue();
            list[0].DeletedOn.Should().Be(dateTime);
        }

        [Fact]
        public async Task EditAsyncShouldEditArticle()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "For test",
                    Content = "For test content",
                    CategoryId = 1,
                    CreatorId = "2",
                    ImageUrl = "ImageUrl",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var mockCloudinaryService = new Mock<ICloudinaryService>();
            mockCloudinaryService
                .Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(Task.Run(() => "ImageUrl"));

            var service = new ArticlesService(mockRepo.Object, mockCloudinaryService.Object);

            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.Setup(_ => _.FileName).Returns("Mockfile.txt");

            var expected = new ArticleInputModel
            {
                Title = "Test",
                Content = "Test content",
                CategoryId = 2,
                CreatorId = "2",
                Id = 2,
                Picture = mockIFormFile.Object,
            };

            await service.EditAsync(expected);

            list[0].Should().BeEquivalentTo(expected, opt => opt.Excluding(prop => prop.Picture));
            list[0].ImageUrl.Should().Be("ImageUrl");
        }

        [Fact]
        public async Task GetAllShouldReturnAllArticlesOrderedByAverageRatingDescendingAndCreatedOn()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Article>()
            {
                new Article
                {
                    Id = 2,
                    Title = "Test 2",
                    CreatedOn = DateTime.Now,
                    ArticleRatings = new List<ArticleRating>(),
                },
                new Article
                {
                    Id = 3,
                    Title = "Test 3",
                    CreatedOn = DateTime.Now.AddDays(3),
                    ArticleRatings = new List<ArticleRating>(),
                },
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    CreatedOn = DateTime.Now,
                    ArticleRatings = new List<ArticleRating>
                    {
                        new ArticleRating { Rating = 1 },
                        new ArticleRating { Rating = 1 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var actual = await service.GetAllAsync<Article>();
            var expected = list
                .OrderByDescending(x => x.ArticleRatings.Any() ? x.ArticleRatings.Average(x => x.Rating) : 0)
                .ThenByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task GetAllShouldSkip1AndTake2ArticlesOrderedByAverageRatingDescendingAndCreatedOn()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Article>()
            {
                new Article
                {
                    Id = 2,
                    Title = "Test 2",
                    CreatedOn = DateTime.Now,
                    ArticleRatings = new List<ArticleRating>(),
                },
                new Article
                {
                    Id = 3,
                    Title = "Test 3",
                    CreatedOn = DateTime.Now.AddDays(3),
                    ArticleRatings = new List<ArticleRating>(),
                },
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    CreatedOn = DateTime.Now,
                    ArticleRatings = new List<ArticleRating>
                    {
                        new ArticleRating { Rating = 1 },
                        new ArticleRating { Rating = 1 },
                    },
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var actual = await service.GetAllAsync<Article>(1, 2);
            var expected = list
                .OrderByDescending(x => x.ArticleRatings.Any() ? x.ArticleRatings.Average(x => x.Rating) : 0)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(1)
                .Take(2);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnArticle()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Article>
            {
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var actual = await service.GetByIdAsync<Article>(1);

            actual.Should().BeEquivalentTo(list[0]);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullIfArticleNotExist()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var actual = await service.GetByIdAsync<Article>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task IsUserCreatorAsyncShouldReturnTrueIfArticleIsAddedByUser()
        {
            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 1,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var userId = "1";
            var actual = await service.IsUserCreatorAsync(userId, 2);

            actual.Should().BeTrue();
        }

        [Fact]
        public async Task IsUserCreatorAsyncShouldReturnFalseIfArticleIsNotAddedByUser()
        {
            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var userId = "2";
            var actual = await service.IsUserCreatorAsync(userId, 2);

            actual.Should().BeFalse();
        }

        [Fact]
        public async Task GetCountAsyncShouldReturnArticlesCount()
        {
            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            var actual = await service.GetCountAsync();

            actual.Should().Be(2);
        }

        [Fact]
        public async Task IsExistAsyncShouldReturnTrueIfArticleExist()
        {
            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            (await service.IsExistAsync(1)).Should().BeTrue();
            (await service.IsExistAsync(2)).Should().BeTrue();
        }

        [Fact]
        public async Task IsExistAsyncShouldReturnFalseIfArticleNotExist()
        {
            var list = new List<Article>
            {
                new Article
                {
                    Id = 2,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    Content = "Test content",
                    CreatorId = "1",
                    CategoryId = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Article>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesService(mockRepo.Object, null);

            (await service.IsExistAsync(3)).Should().BeFalse();
            (await service.IsExistAsync(4)).Should().BeFalse();
        }
    }
}
