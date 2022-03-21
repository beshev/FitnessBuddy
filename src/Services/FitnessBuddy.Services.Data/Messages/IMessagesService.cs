namespace FitnessBuddy.Services.Data.Messages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        public Task SendMessageAsync(string authorId, string receiverId, string content);

        public IEnumerable<TModel> GetMessages<TModel>(string firstUserId, string secondUserId);

        public Task DeleteMessageAsync(string authorId, string receiverId);

        public IEnumerable<TModel> GetConversations<TModel>(string userId);
    }
}
