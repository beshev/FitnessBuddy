namespace FitnessBuddy.Services.Data.Messages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task DeleteMessageAsync(string authorId, string receiverId)
        {
            var message = this.messagesRepository
                .All()
                .FirstOrDefault(x => x.AuthorId == authorId && x.ReceiverId == receiverId);

            this.messagesRepository.Delete(message);

            await this.messagesRepository.SaveChangesAsync();
        }

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
                .AsEnumerable();

            return conversationsWithUsers;
        }

        public IEnumerable<TModel> GetMessages<TModel>(string firstUserId, string secondUserId)
            => this.messagesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x =>
                (x.AuthorId == firstUserId && x.ReceiverId == secondUserId)
                || (x.AuthorId == secondUserId && x.ReceiverId == firstUserId))
            .OrderBy(x => x.CreatedOn)
            .To<TModel>()
            .AsEnumerable();

        public async Task SendMessageAsync(string authorId, string receiverId, string content)
        {
            var message = new Message
            {
                AuthorId = authorId,
                ReceiverId = receiverId,
                Content = content,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();
        }
    }
}
