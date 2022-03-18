namespace FitnessBuddy.Services.Data.ExercisesLikes
{
    using System.Threading.Tasks;

    public interface IExercisesLikesService
    {
        public Task LikeAsync(string userId, int exerciseId);

        public Task UnLikeAsync(string userId, int exerciseId);

        public bool IsExists(string userId, int exerciseId);

        public int GetExerciseLikesCount(int exerciseId);
    }
}
