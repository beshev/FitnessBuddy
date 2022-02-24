namespace FitnessBuddy.Services.Data.Replies
{
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Replies;

    public interface IRepliesService
    {
        public Task AddAsync(ReplyInputModel model);

        public Task DeleteAsync(int id);

        public TModel GetById<TModel>(int id);

        public bool IsUserAuthor(int replyId, string userId);

        public bool IsExist(int replyId);
    }
}
