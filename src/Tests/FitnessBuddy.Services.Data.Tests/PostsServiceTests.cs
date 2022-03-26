namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Web.ViewModels.Posts;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class PostsServiceTests
    {
        [Fact]
        public async Task CreateAsyncShouldAddPost()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>();

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>())).Callback((Post post) => list.Add(post));

            var service = new PostsService(mockRepo.Object);

            var post = new PostInputModel
            {
                Id = 1,
                AuthorId = "2",
                CategoryId = 3,
                Description = "Test desc...",
                Title = "Testing is so booooring!!",
            };

            await service.CreateAsync(post);

            var actual = list.FirstOrDefault();

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(post);
        }

        [Fact]
        public async Task DeleteAsyncShouldSetPostIsDeleteToTrue()
        {
            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo
                .Setup(x => x.Delete(It.IsAny<Post>()))
                .Callback((Post post) =>
            {
                post.IsDeleted = true;
                post.DeletedOn = dateTime;
            });

            var service = new PostsService(mockRepo.Object);

            await service.DeleteAsync(1);

            var actual = list.FirstOrDefault();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }

        [Fact]
        public async Task EditAsyncShouldEditPost()
        {
            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var editedPost = new PostInputModel
            {
                Id = 1,
                CategoryId = 5,
                Title = "Testing EDITED",
                Description = "Test desc.. EDITED",
            };

            await service.EditAsync(editedPost);

            var actual = list.FirstOrDefault();

            actual.Should().BeEquivalentTo(editedPost);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdAsyncShouldReturnPost(int postId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 4,
                   Title = "Testing 2",
                   Description = "Test desc.. 2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetByIdAsync<Post>(postId);
            var expected = list.FirstOrDefault(x => x.Id == postId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetByIdAsyncShouldReturnNullIfPostDoesNotExist(int postId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 4,
                   Title = "Testing 2",
                   Description = "Test desc.. 2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetByIdAsync<Post>(postId);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IncreaseViewsAsyncShouldIncreaseViewsOfPost(int postId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 5,
                   Title = "Testing 2",
                   Description = "Test desc.. 2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = list.FirstOrDefault(x => x.Id == postId);

            actual.Views.Should().Be(0);

            await service.IncreaseViewsAsync(postId);
            actual.Views.Should().Be(1);

            await service.IncreaseViewsAsync(postId);
            actual.Views.Should().Be(2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnTrueIfPostExist(int postId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 5,
                   Title = "Testing 2",
                   Description = "Test desc.. 2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.IsExistAsync(postId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IsExistAsyncShouldReturnFalseIfPostDoesNotExist(int postId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 5,
                   Title = "Testing 2",
                   Description = "Test desc.. 2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.IsExistAsync(postId);

            actual.Should().BeFalse();
        }

        [Fact]
        public async Task GetCountAsyncShouldReturnAllPostsCount()
        {
            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 2,
                   Title = "Testing",
                   Description = "Test desc..",
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 5,
                   Title = "Testing 2",
                   Description = "Test desc.. 2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetCountAsync();

            actual.Should().Be(list.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetCountAsyncShouldReturnAllPostsCountInGivenCategory(int categoryId)
        {
            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 2,
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 4,
                   CategoryId = 2,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetCountAsync(categoryId);
            var expected = list.Count(x => x.CategoryId == categoryId);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public async Task IsUserAuthorAsyncShouldReturnTrueIfUserIsAuthorOfPost(string userId, int postId)
        {
            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   AuthorId = "1",
                },
                new Post
                {
                   Id = 2,
                   AuthorId = "2",
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.IsUserAuthorAsync(postId, userId);

            actual.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllPostsOrderedByViewsThenByCreatedOnDescending()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   Views = 2,
                   CreatedOn = DateTime.Now.AddDays(1),
                },
                new Post
                {
                   Id = 2,
                   Views = 2,
                   CreatedOn = DateTime.Now,
                },
                new Post
                {
                   Id = 3,
                   Views = 4,
                   CreatedOn = DateTime.Now,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetAllAsync<Post>();
            var expected = list
                .OrderByDescending(x => x.Views)
                .ThenByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllAsyncShouldReturnAllPostsFromGivenCategoryOrderedByViewsThenByCreatedOnDescending(int categoryId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   Views = 2,
                   CreatedOn = DateTime.Now.AddDays(1),
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 2,
                   Views = 2,
                   CreatedOn = DateTime.Now,
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 3,
                   Views = 4,
                   CreatedOn = DateTime.Now,
                   CategoryId = 2,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetAllAsync<Post>(categoryId);
            var expected = list
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.Views)
                .ThenByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 0, 1)]
        [InlineData(3, 1, 1)]
        public async Task GetAllAsyncShouldReturnSkipAndTakeCountOfPostsFromGivenCategoryOrderedByViewsThenByCreatedOnDescending(int categoryId, int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   Views = 2,
                   CreatedOn = DateTime.Now.AddDays(1),
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 2,
                   Views = 2,
                   CreatedOn = DateTime.Now,
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 5,
                   Views = 5,
                   CreatedOn = DateTime.Now,
                   CategoryId = 1,
                },
                new Post
                {
                   Id = 3,
                   Views = 4,
                   CreatedOn = DateTime.Now,
                   CategoryId = 2,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetAllAsync<Post>(categoryId, skip, take);
            var expected = list
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.Views)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 0)]
        [InlineData(0, 3)]
        [InlineData(0, 0)]
        public async Task GetAllAsyncShouldReturnSkipAndTakeCountOfPostsOrderedByViewsThenByCreatedOnDescending(int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Post>()
            {
                new Post
                {
                   Id = 1,
                   Views = 2,
                   CreatedOn = DateTime.Now.AddDays(1),
                },
                new Post
                {
                   Id = 2,
                   Views = 2,
                   CreatedOn = DateTime.Now,
                },
                new Post
                {
                   Id = 5,
                   Views = 5,
                   CreatedOn = DateTime.Now,
                },
                new Post
                {
                   Id = 3,
                   Views = 4,
                   CreatedOn = DateTime.Now,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Post>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new PostsService(mockRepo.Object);

            var actual = await service.GetAllAsync<Post>(null, skip, take);
            var expected = list
                .OrderByDescending(x => x.Views)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }
    }
}
