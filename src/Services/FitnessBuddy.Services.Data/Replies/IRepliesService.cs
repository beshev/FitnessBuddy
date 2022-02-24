namespace FitnessBuddy.Services.Data.Replies
{
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Replies;

    public interface IRepliesService
    {
        public Task AddAsync(ReplyInputModel model);

        public Task DeleteAsync(int id);

        public TModel GetById<TModel>(int id);
    }
}
