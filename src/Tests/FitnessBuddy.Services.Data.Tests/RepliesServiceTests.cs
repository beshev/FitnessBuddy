namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Replies;
    using FitnessBuddy.Web.ViewModels.Replies;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class RepliesServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddReply()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Reply>();

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Reply>())).Callback((Reply reply) => list.Add(reply));

            var service = new RepliesService(mockRepo.Object);

            var reply = new ReplyInputModel
            {
                AuthorId = "1",
                Description = "test",
                ParentId = null,
                PostId = 1,
            };

            await service.AddAsync(reply);

            var actual = list.FirstOrDefault();

            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(reply);
        }

        [Fact]
        public async Task DeleteAsyncShouldSetIsDeletedToTrue()
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    IsDeleted = false,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo
                .Setup(x => x.Delete(It.IsAny<Reply>()))
                .Callback((Reply reply) =>
                {
                    reply.IsDeleted = true;
                    reply.DeletedOn = dateTime;
                });

            var service = new RepliesService(mockRepo.Object);

            await service.DeleteAsync(1);

            var actual = list.FirstOrDefault();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }

        [Fact]
        public async Task EditAsyncShouldEditReply()
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    Description = "test",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var editedReply = new ReplyEditInputModel
            {
                Id = 1,
                Description = "test edited!!",
            };

            await service.EditAsync(editedReply);

            var actual = list.FirstOrDefault();

            actual.Should().BeEquivalentTo(editedReply);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllRepliesOrderdByCreatedOn()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    Description = "test",
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new Reply
                {
                    Id = 2,
                    Description = "test",
                    CreatedOn = DateTime.Now,
                },
                new Reply
                {
                    Id = 3,
                    Description = "test",
                    CreatedOn = DateTime.Now.AddDays(2),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.GetAllAsync<Reply>();
            var expected = list.OrderByDescending(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task GetAllAsyncShouldSkipAndTakeCountOfRepliesOrderdByCreatedOn(int skip, int take)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    Description = "test",
                    CreatedOn = DateTime.Now.AddDays(1),
                },
                new Reply
                {
                    Id = 2,
                    Description = "test",
                    CreatedOn = DateTime.Now,
                },
                new Reply
                {
                    Id = 3,
                    Description = "test",
                    CreatedOn = DateTime.Now.AddDays(2),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.GetAllAsync<Reply>(skip, take);
            var expected = list
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(take);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdAsyncShouldReturnReplyById(int replyId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                },
                new Reply
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.GetByIdAsync<Reply>(replyId);
            var expected = list.FirstOrDefault(x => x.Id == replyId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCountAsyncShouldReturnRepliesCount()
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                },
                new Reply
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.GetCountAsync();

            actual.Should().Be(2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetReplyPostIdAsyncShouldReturnPostIdOfReply(int replyId)
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    PostId = 2,
                },
                new Reply
                {
                    Id = 2,
                    PostId = 3,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.GetReplyPostIdAsync(replyId);
            var expected = list.FirstOrDefault(x => x.Id == replyId)?.PostId;

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public async Task GetReplyPostIdAsyncShouldReturn0IfThereIsNotReplyWithGivenId(int replyId)
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    PostId = 2,
                },
                new Reply
                {
                    Id = 2,
                    PostId = 3,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.GetReplyPostIdAsync(replyId);

            actual.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task IsExistAsyncShouldReturnTrueIfReplyExist(int replyId)
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                },
                new Reply
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.IsExistAsync(replyId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IsExistAsyncShouldReturnFalseIfReplyNotExist(int replyId)
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                },
                new Reply
                {
                    Id = 2,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.IsExistAsync(replyId);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, "2")]
        [InlineData(2, "1")]
        public async Task IsUserAuthorAsyncShouldReturnTrueIfReplyIsWritenByUser(int replyId, string userId)
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    AuthorId = "2",
                },
                new Reply
                {
                    Id = 2,
                    AuthorId = "1",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.IsUserAuthorAsync(replyId, userId);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task IsUserAuthorAsyncShouldReturnFalseIfReplyIsNotWritenByUser(int replyId, string userId)
        {
            var list = new List<Reply>()
            {
                new Reply
                {
                    Id = 1,
                    AuthorId = "2",
                },
                new Reply
                {
                    Id = 2,
                    AuthorId = "1",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Reply>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new RepliesService(mockRepo.Object);

            var actual = await service.IsUserAuthorAsync(replyId, userId);

            actual.Should().BeFalse();
        }
    }
}
