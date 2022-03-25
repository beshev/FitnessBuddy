namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Articles;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Xunit;

    public class ArticleCategoriesServiceTests
    {
        [Fact]
        public async Task GetAllMethodShouldReturnAllCategories()
        {
            TestMapper.InitializeAutoMapper();

            var expected = new List<ArticleCategory>
            {
                new ArticleCategory { ImageUrl = "123", Name = "Test", Id = 1 },
                new ArticleCategory { ImageUrl = "321", Name = "Test 2", Id = 1 },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ArticleCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(expected.AsQueryable().BuildMock());

            var service = new ArticleCategoriesService(mockRepo.Object);

            var actual = await service.GetAllAsync<ArticleCategory>();

            actual.Should().HaveCount(2);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAllMethodShouldReturnEmptyCollection()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ArticleCategory>();

            var mockRepo = MockRepo.MockDeletableRepository<ArticleCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticleCategoriesService(mockRepo.Object);

            var actual = await service.GetAllAsync<ArticleCategory>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCategoryArticlesShouldReturnAllCategoryArticles()
        {
            TestMapper.InitializeAutoMapper();

            var articles = new List<Article>
            {
                new Article
                {
                    Id = 1,
                    Title = "Test",
                    CategoryId = 1,
                },
                new Article
                {
                    Id = 2,
                    Title = "Test 2",
                    CategoryId = 1,
                },
            };

            var categories = new List<ArticleCategory>
            {
                new ArticleCategory { Id = 1, Name = "TestCategory",  Articles = articles },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ArticleCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(categories.AsQueryable().BuildMock());

            var service = new ArticleCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryArticlesAsync<Article>(categories[0].Name);

            actual.Should().HaveCount(2);
            actual.Should().BeEquivalentTo(articles);
        }

        [Fact]
        public async Task GetCategoryArticlesShouldReturnEmptyCollectionIfCategoryDontHaveArticles()
        {
            TestMapper.InitializeAutoMapper();

            var categories = new List<ArticleCategory>
            {
                new ArticleCategory { Id = 2, Name = "TestCategory",  Articles = new List<Article>() },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ArticleCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(categories.AsQueryable().BuildMock());

            var service = new ArticleCategoriesService(mockRepo.Object);

            var actual = await service.GetCategoryArticlesAsync<Article>(categories[0].Name);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task IsExistShouldReturnTrueIfCategoryExist()
        {
            var list = new List<ArticleCategory>
            {
                new ArticleCategory { Name = "Test" },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ArticleCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticleCategoriesService(mockRepo.Object);

            (await service.IsExistAsync("Test")).Should().BeTrue();
        }

        [Fact]
        public async Task IsExistShouldReturnFalseIfCategoryDontExist()
        {
            var list = new List<ArticleCategory>
            {
                new ArticleCategory { Name = "Test" },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ArticleCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticleCategoriesService(mockRepo.Object);

            (await service.IsExistAsync("Test2")).Should().BeFalse();
        }
    }
}
