namespace FitnessBuddy.Services.Data.Messages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        public TModel GetById<TModel>(int id);

        public Task<int> SendMessageAsync(string authorId, string receiverId, string content);

        public IEnumerable<TModel> GetMessages<TModel>(string firstUserId, string secondUserId);

        public Task DeleteMessageAsync(int id);

        public IEnumerable<TModel> GetConversations<TModel>(string userId);

        public string GetLastActivity(string authorId, string receiverId);
    }
}
