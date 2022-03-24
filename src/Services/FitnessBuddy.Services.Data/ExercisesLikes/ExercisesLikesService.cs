namespace FitnessBuddy.Services.Data.ExercisesLikes
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExercisesLikesService : IExercisesLikesService
    {
        private readonly IRepository<ExerciseLike> exercisesLikesRepository;

        public ExercisesLikesService(IRepository<ExerciseLike> exercisesLikesRepository)
        {
            this.exercisesLikesRepository = exercisesLikesRepository;
        }

        public async Task<int> GetExerciseLikesCountAsync(int exerciseId)
            => await this.exercisesLikesRepository
            .AllAsNoTracking()
            .Where(x => x.ExerciseId == exerciseId)
            .CountAsync();

        public async Task<bool> IsExists(string userId, int exerciseId)
            => await this.exercisesLikesRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.ExerciseId == exerciseId);

        public async Task LikeAsync(string userId, int exerciseId)
        {
            var exerciseLike = new ExerciseLike
            {
                UserId = userId,
                ExerciseId = exerciseId,
            };

            await this.exercisesLikesRepository.AddAsync(exerciseLike);
            await this.exercisesLikesRepository.SaveChangesAsync();
        }

        public async Task UnLikeAsync(string userId, int exerciseId)
        {
            var exerciseLike = this.exercisesLikesRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.UserId == userId && x.ExerciseId == exerciseId);

            this.exercisesLikesRepository.Delete(exerciseLike);
            await this.exercisesLikesRepository.SaveChangesAsync();
        }
    }
}
