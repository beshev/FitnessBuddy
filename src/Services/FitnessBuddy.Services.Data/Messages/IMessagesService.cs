namespace FitnessBuddy.Services.Data.Messages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        public Task<TModel> GetByIdAsync<TModel>(int id);

        public Task<int> SendMessageAsync(string authorId, string receiverId, string content);

        public Task<IEnumerable<TModel>> GetMessagesAsync<TModel>(string firstUserId, string secondUserId);

        public Task DeleteMessageAsync(int id);

        public Task<IEnumerable<TModel>> GetConversationsAsync<TModel>(string userId);

        public Task<string> GetLastActivityAsync(string authorId, string receiverId);
    }
}
