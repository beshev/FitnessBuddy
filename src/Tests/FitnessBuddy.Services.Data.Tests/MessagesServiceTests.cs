namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Messages;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class MessagesServiceTests
    {
        [Fact]
        public async Task DeleteMessageAsyncShouldSetIsDeletedToTrue()
        {
            var list = new List<Message>()
            {
                new Message
                {
                    Id = 1,
                },
            };

            var dateTime = DateTime.Now;

            var mockRepo = MockRepo.MockDeletableRepository<Message>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());
            mockRepo
                .Setup(x => x.Delete(It.IsAny<Message>()))
                .Callback((Message message) =>
                {
                    message.IsDeleted = true;
                    message.DeletedOn = dateTime;
                });

            var service = new MessagesService(mockRepo.Object);

            await service.DeleteMessageAsync(1);

            var actual = list.FirstOrDefault();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTime);
        }

        [Fact]
        public async Task SendMessageAsyncShouldAddMessage()
        {
            var list = new List<Message>();

            var mockRepo = MockRepo.MockDeletableRepository<Message>();
            mockRepo
                .Setup(x => x.AddAsync(It.IsAny<Message>()))
                .Callback((Message message) => list.Add(message));

            var service = new MessagesService(mockRepo.Object);

            var authorId = "1";
            var receiverId = "2";
            var content = "Test";

            await service.SendMessageAsync(authorId, receiverId, content);

            var actual = list.FirstOrDefault();

            actual.AuthorId.Should().Be(authorId);
            actual.ReceiverId.Should().Be(receiverId);
            actual.Content.Should().Be(content);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdAsyncShouldReturnMessageById(int messageId)
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Message>()
            {
                new Message
                {
                    Id = 1,
                    Content = "test",
                },
                new Message
                {
                    Id = 2,
                    Content = "test content",
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Message>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MessagesService(mockRepo.Object);

            var actual = await service.GetByIdAsync<Message>(messageId);
            var expected = list.FirstOrDefault(x => x.Id == messageId);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetConversationsAsyncShouldReturnUserAllConversations()
        {
            TestMapper.InitializeAutoMapper();

            var jack = new ApplicationUser { UserName = "Jack" };
            var jimmy = new ApplicationUser { UserName = "Jimmy" };

            var list = new List<Message>()
            {
                new Message
                {
                    Id = 1,
                    AuthorId = "1",
                    Receiver = jack,
                    Content = "test",
                },
                new Message
                {
                    Id = 2,
                    Content = "test content",
                    ReceiverId = "1",
                    Author = jimmy,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Message>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable().BuildMock());

            var service = new MessagesService(mockRepo.Object);

            var actual = await service.GetConversationsAsync<ApplicationUser>("1");
            var expected = new ApplicationUser[] { jack, jimmy };

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLastActivityAsyncShouldReturnTheTimeOfTheLastSendedMessageBetweenTwoUsers()
        {
            TestMapper.InitializeAutoMapper();

            var lastSendedMessageTime = DateTime.Now.AddDays(1);

            var list = new List<Message>()
            {
                new Message
                {
                    Id = 1,
                    AuthorId = "1",
                    Content = "test",
                    ReceiverId = "2",
                    CreatedOn = DateTime.Now,
                },
                new Message
                {
                    Id = 2,
                    Content = "test content",
                    ReceiverId = "1",
                    AuthorId = "2",
                    CreatedOn = lastSendedMessageTime,
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Message>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new MessagesService(mockRepo.Object);

            var actual = await service.GetLastActivityAsync("1", "2");
            var expected = lastSendedMessageTime.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetMessagesAsyncShouldReturnAllSendedMessageWithDeletedBetweenTwoUsersOrderdByCreatedOn()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<Message>()
            {
                new Message
                {
                    Id = 1,
                    AuthorId = "1",
                    Content = "test",
                    ReceiverId = "2",
                    CreatedOn = DateTime.Now,
                },
                new Message
                {
                    Id = 2,
                    Content = "test content",
                    ReceiverId = "1",
                    AuthorId = "2",
                    CreatedOn = DateTime.Now.AddDays(-1),
                },
            };

            var mockRepo = MockRepo.MockDeletableRepository<Message>();
            mockRepo.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(list.AsQueryable().BuildMock());

            var service = new MessagesService(mockRepo.Object);

            var firstUser = "1";
            var secondUser = "2";

            var actual = await service.GetMessagesAsync<Message>(firstUser, secondUser);
            var expected = list
                .Where(x =>
                (x.AuthorId == firstUser && x.ReceiverId == secondUser)
                || (x.AuthorId == secondUser && x.ReceiverId == firstUser))
                .OrderBy(x => x.CreatedOn);

            actual.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }
    }
}
