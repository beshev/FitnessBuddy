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
    using Microsoft.EntityFrameworkCore;

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

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.messagesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetConversationsAsync<TModel>(string userId)
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

            var conversationsWithUsers = await sendMessages
                .Concat(receivedMessages)
                .Where(x => x.Id != userId)
                .Distinct()
                .To<TModel>()
                .ToListAsync();

            return conversationsWithUsers;
        }

        public async Task<string> GetLastActivityAsync(string authorId, string receiverId)
             => (await this.messagesRepository
            .AllAsNoTracking()
            .Where(x =>
                    (x.AuthorId == authorId && x.ReceiverId == receiverId)
                    || (x.AuthorId == receiverId && x.ReceiverId == authorId))
            .OrderByDescending(x => x.CreatedOn)
            .Select(x => x.CreatedOn)
            .FirstOrDefaultAsync())
            .ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture);

        public async Task<IEnumerable<TModel>> GetMessagesAsync<TModel>(string firstUserId, string secondUserId)
            => await this.messagesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x =>
                (x.AuthorId == firstUserId && x.ReceiverId == secondUserId)
                || (x.AuthorId == secondUserId && x.ReceiverId == firstUserId))
            .OrderBy(x => x.CreatedOn)
            .To<TModel>()
            .ToListAsync();

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
