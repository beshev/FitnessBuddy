namespace FitnessBuddy.Services.Data.Replies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Replies;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(int skip = 0, int? take = null)
        {
            IQueryable<Reply> query = this.repliesRepository
            .AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn);

            if (take.HasValue)
            {
                query = query.Skip(skip).Take(take.Value);
            }

            return await query
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.repliesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefaultAsync();

        public async Task<int> GetCountAsync()
            => await this.repliesRepository
            .AllAsNoTracking()
            .CountAsync();

        public async Task<int> GetReplyPostIdAsync(int replyId)
            => await this.repliesRepository
            .AllAsNoTracking()
            .Where(x => x.Id == replyId)
            .Select(x => x.PostId)
            .FirstOrDefaultAsync();

        public async Task<bool> IsExistAsync(int replyId)
            => await this.repliesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == replyId);

        public async Task<bool> IsUserAuthorAsync(int replyId, string userId)
            => await this.repliesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == replyId & x.AuthorId == userId);
    }
}
