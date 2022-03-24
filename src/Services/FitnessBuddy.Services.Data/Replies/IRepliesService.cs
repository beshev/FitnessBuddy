namespace FitnessBuddy.Services.Data.Replies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Replies;

    public interface IRepliesService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>(int skip = 0, int? take = null);

        public Task AddAsync(ReplyInputModel model);

        public Task DeleteAsync(int id);

        public Task EditAsync(ReplyEditInputModel model);

        public Task<int> GetReplyPostIdAsync(int replyId);

        public Task<int> GetCountAsync();

        public Task<TModel> GetByIdAsync<TModel>(int id);

        public Task<bool> IsUserAuthorAsync(int replyId, string userId);

        public Task<bool> IsExistAsync(int replyId);
    }
}
