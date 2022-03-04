namespace FitnessBuddy.Services.Data.Replies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Replies;

    public interface IRepliesService
    {
        public IEnumerable<TModel> GetAll<TModel>(int skip = 0, int? take = null);

        public Task AddAsync(ReplyInputModel model);

        public Task DeleteAsync(int id);

        public Task EditAsync(ReplyEditInputModel model);

        public int GetReplyPostId(int replyId);

        public int GetCount();

        public TModel GetById<TModel>(int id);

        public bool IsUserAuthor(int replyId, string userId);

        public bool IsExist(int replyId);
    }
}
