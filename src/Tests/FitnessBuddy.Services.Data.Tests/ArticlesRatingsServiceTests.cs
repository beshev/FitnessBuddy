namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.ArticlesRatings;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ArticlesRatingsServiceTests
    {
        [Theory]
        [InlineData(1.5, 2.5)]
        [InlineData(1.5, 1.7)]
        [InlineData(3, 3)]
        public async Task CalcAvgRateAsyncShouldCalculateArticleAverageRating(int ratingOne, int ratingTwo)
        {
            var list = new List<ArticleRating>
            {
                new ArticleRating
                {
                    UserId = "2",
                    ArticleId = 1,
                    Rating = ratingOne,
                },
                new ArticleRating
                {
                    UserId = "2",
                    ArticleId = 1,
                    Rating = ratingTwo,
                },
                new ArticleRating
                {
                    UserId = "2",
                    ArticleId = 2,
                    Rating = 1,
                },
            };

            var mockRepo = MockRepo.MockRepository<ArticleRating>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesRatingsService(mockRepo.Object);

            var actual = await service.CalcAvgRateAsync(1);
            var expected = (ratingOne + ratingTwo) / 2;

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1.5, 2.5)]
        [InlineData(1.5, 1.7)]
        [InlineData(3, 3)]
        public async Task CalcAvgRateAsyncShouldReturn0IfArticleDontHaveRating(int ratingOne, int ratingTwo)
        {
            var list = new List<ArticleRating>
            {
                new ArticleRating
                {
                    UserId = "2",
                    ArticleId = 1,
                    Rating = ratingOne,
                },
                new ArticleRating
                {
                    UserId = "2",
                    ArticleId = 1,
                    Rating = ratingTwo,
                },
            };

            var mockRepo = MockRepo.MockRepository<ArticleRating>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ArticlesRatingsService(mockRepo.Object);

            var actual = await service.CalcAvgRateAsync(2);

            actual.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 5)]
        public async Task RateAsyncShouldAddArticleRatingIfUserRateArticleForFirstTime(int articleId, int rating)
        {
            var list = new List<ArticleRating>();

            var mockRepo = MockRepo.MockRepository<ArticleRating>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<ArticleRating>()))
                .Callback((ArticleRating articleRating) => list.Add(articleRating));

            var service = new ArticlesRatingsService(mockRepo.Object);

            var userId = "1";

            list.Should().HaveCount(0);

            await service.RateAsync(articleId, userId, rating);

            list.Should().HaveCount(1);
            list[0].Rating.Should().Be(rating);
            list[0].ArticleId.Should().Be(articleId);
            list[0].UserId.Should().Be(userId);
        }

        [Theory]
        [InlineData(1, 3, 5)]
        [InlineData(2, 5, 3)]
        public async Task RateAsyncShouldChangeArticleRatingIfUserIsAlreadyRatedArticle(int articleId, int firstRate, int secondRate)
        {
            var list = new List<ArticleRating>();

            var mockRepo = MockRepo.MockRepository<ArticleRating>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<ArticleRating>()))
                .Callback((ArticleRating articleRating) => list.Add(articleRating));

            var service = new ArticlesRatingsService(mockRepo.Object);

            var userId = "1";

            await service.RateAsync(articleId, userId, firstRate);

            list.Should().HaveCount(1);

            await service.RateAsync(articleId, userId, secondRate);

            list.Should().HaveCount(1);
            list[0].Rating.Should().Be(secondRate);
        }
    }
}
