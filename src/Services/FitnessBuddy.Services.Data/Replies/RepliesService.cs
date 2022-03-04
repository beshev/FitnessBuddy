namespace FitnessBuddy.Services.Data.Replies
{
    using System.Collections.Generic;
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

        public async Task EditAsync(ReplyEditInputModel model)
        {
            var reply = this.repliesRepository
                .All()
                .FirstOrDefault(x => x.Id == model.Id);

            reply.Description = model.Description;

            await this.repliesRepository.SaveChangesAsync();
        }

        public IEnumerable<TModel> GetAll<TModel>(int skip = 0, int? take = null)
        {
            IQueryable<Reply> query = this.repliesRepository
            .AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn);

            if (take.HasValue)
            {
                query = query.Skip(skip).Take(take.Value);
            }

            return query
                .To<TModel>()
                .AsEnumerable();
        }

        public TModel GetById<TModel>(int id)
            => this.repliesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        public int GetCount()
            => this.repliesRepository
            .AllAsNoTracking()
            .Count();

        public int GetReplyPostId(int replyId)
            => this.repliesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == replyId)
            .Select(x => x.PostId)
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
