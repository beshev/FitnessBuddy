namespace FitnessBuddy.Services.Data.ExercisesLikes
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;

    public class ExercisesLikesService : IExercisesLikesService
    {
        private readonly IRepository<ExerciseLike> exercisesLikesRepository;

        public ExercisesLikesService(IRepository<ExerciseLike> exercisesLikesRepository)
        {
            this.exercisesLikesRepository = exercisesLikesRepository;
        }

        public bool IsExists(string userId, int exerciseId)
            => this.exercisesLikesRepository
            .AllAsNoTracking()
            .Any(x => x.UserId == userId && x.ExerciseId == exerciseId);

        public async Task Like(string userId, int exerciseId)
        {
            var exerciseLike = new ExerciseLike
            {
                UserId = userId,
                ExerciseId = exerciseId,
            };

            await this.exercisesLikesRepository.AddAsync(exerciseLike);
            await this.exercisesLikesRepository.SaveChangesAsync();
        }

        public async Task UnLike(string userId, int exerciseId)
        {
            var exerciseLike = this.exercisesLikesRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.UserId == userId && x.ExerciseId == exerciseId);

            this.exercisesLikesRepository.Delete(exerciseLike);
            await this.exercisesLikesRepository.SaveChangesAsync();
        }
    }
}
