namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Posts;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Xunit;

    public class PostCategoriesServiceTests
    {
        [Fact]
        public async Task GetAllAsyncShouldReturnAllPostCategories()
        {
            TestMapper.InitializeAutoMapper();

            var lits = new List<PostCategory>()
            {
                new PostCategory { Name = "Test" },
                new PostCategory { Name = "Test 2" },
            };

            var mockRepo = MockRepo.MockDeletableRepository<PostCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(lits.AsQueryable().BuildMock());

            var service = new PostCategoriesService(mockRepo.Object);

            var actual = await service.GetAllAsync<PostCategory>();

            actual.Should().HaveCount(2);
            actual.Should().BeEquivalentTo(lits);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetNameAsyncShouldReturnCategoryNameFromPassedId(int categoryId)
        {
            TestMapper.InitializeAutoMapper();

            var lits = new List<PostCategory>()
            {
                new PostCategory
                {
                    Name = "Test",
                    Id = 1,
                },
                new PostCategory
                {
                    Name = "Test 2",
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<PostCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(lits.AsQueryable().BuildMock());

            var service = new PostCategoriesService(mockRepo.Object);

            var actual = await service.GetNameAsync(categoryId);
            var expected = lits.FirstOrDefault(x => x.Id == categoryId)?.Name;

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnTrueIfCategoryExist(int categoryId)
        {
            TestMapper.InitializeAutoMapper();

            var lits = new List<PostCategory>()
            {
                new PostCategory
                {
                    Name = "Test",
                    Id = 1,
                },
                new PostCategory
                {
                    Name = "Test 2",
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<PostCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(lits.AsQueryable().BuildMock());

            var service = new PostCategoriesService(mockRepo.Object);

            var actual = await service.IsExistAsync(categoryId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IsExistAsyncShouldReturnFalseIfCategoryDoesNotExist(int categoryId)
        {
            TestMapper.InitializeAutoMapper();

            var lits = new List<PostCategory>()
            {
                new PostCategory
                {
                    Name = "Test",
                    Id = 1,
                },
                new PostCategory
                {
                    Name = "Test 2",
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<PostCategory>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(lits.AsQueryable().BuildMock());

            var service = new PostCategoriesService(mockRepo.Object);

            var actual = await service.IsExistAsync(categoryId);

            actual.Should().BeFalse();
        }
    }
}
