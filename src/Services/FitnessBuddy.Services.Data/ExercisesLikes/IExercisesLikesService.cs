namespace FitnessBuddy.Services.Data.ExercisesLikes
{
    using System.Threading.Tasks;

    public interface IExercisesLikesService
    {
        public Task LikeAsync(string userId, int exerciseId);

        public Task UnLikeAsync(string userId, int exerciseId);

        public Task<bool> IsExistsAsync(string userId, int exerciseId);

        public Task<int> GetExerciseLikesCountAsync(int exerciseId);
    }
}
