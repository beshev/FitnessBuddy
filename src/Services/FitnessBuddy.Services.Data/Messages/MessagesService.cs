namespace FitnessBuddy.Services.Data.Messages
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessagesService(
            IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task DeleteMessageAsync(int id)
        {
            var message = this.messagesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            this.messagesRepository.Delete(message);

            await this.messagesRepository.SaveChangesAsync();
        }

        public TModel GetById<TModel>(int id)
            => this.messagesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        public IEnumerable<TModel> GetConversations<TModel>(string userId)
        {
            var sendMessages = this.messagesRepository
                .All()
                .Where(x => x.AuthorId == userId)
                .OrderBy(x => x.CreatedOn)
                .Select(x => x.Receiver);

            var receivedMessages = this.messagesRepository
                .All()
                .Where(x => x.ReceiverId == userId)
                .OrderBy(x => x.CreatedOn)
                .Select(x => x.Author);

            var conversationsWithUsers = sendMessages
                .Concat(receivedMessages)
                .Where(x => x.Id != userId)
                .Distinct()
                .To<TModel>()
                .ToList();

            return conversationsWithUsers;
        }

        public string GetLastActivity(string authorId, string receiverId)
             => this.messagesRepository
            .AllAsNoTracking()
            .Where(x =>
                    (x.AuthorId == authorId && x.ReceiverId == receiverId)
                    || (x.AuthorId == receiverId && x.ReceiverId == authorId))
            .OrderByDescending(x => x.CreatedOn)
            .Select(x => x.CreatedOn)
            .FirstOrDefault()
            .ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture);

        public IEnumerable<TModel> GetMessages<TModel>(string firstUserId, string secondUserId)
            => this.messagesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x =>
                (x.AuthorId == firstUserId && x.ReceiverId == secondUserId)
                || (x.AuthorId == secondUserId && x.ReceiverId == firstUserId))
            .OrderBy(x => x.CreatedOn)
            .To<TModel>()
            .AsEnumerable();

        public bool IsExist(int id)
            => this.messagesRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id);

        public async Task<int> SendMessageAsync(string authorId, string receiverId, string content)
        {
            var message = new Message
            {
                AuthorId = authorId,
                ReceiverId = receiverId,
                Content = content,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();

            return message.Id;
        }
    }
}
