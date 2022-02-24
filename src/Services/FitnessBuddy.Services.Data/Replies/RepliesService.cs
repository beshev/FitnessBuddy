namespace FitnessBuddy.Services.Data.Replies
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Replies;

    public class RepliesService : IRepliesService
    {
        private readonly IDeletableEntityRepository<Reply> repliesRepository;

        public RepliesService(IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.repliesRepository = repliesRepository;
        }

        public async Task AddAsync(ReplyInputModel model)
        {
            var reply = AutoMapperConfig.MapperInstance.Map<Reply>(model);

            await this.repliesRepository.AddAsync(reply);
            await this.repliesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reply = this.repliesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            this.repliesRepository.Delete(reply);
            await this.repliesRepository.SaveChangesAsync();
        }

        public TModel GetById<TModel>(int id)
            => this.repliesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        public bool IsExist(int replyId)
            => this.repliesRepository
            .AllAsNoTracking()
            .Any(x => x.Id == replyId);

        public bool IsUserAuthor(int replyId, string userId)
            => this.repliesRepository
            .AllAsNoTracking()
            .Any(x => x.Id == replyId & x.AuthorId == userId);
    }
}
