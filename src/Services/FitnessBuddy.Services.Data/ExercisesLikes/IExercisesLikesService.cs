namespace FitnessBuddy.Services.Data.ExercisesLikes
{
    using System.Threading.Tasks;

    public interface IExercisesLikesService
    {
        public Task Like(string userId, int exerciseId);

        public Task UnLike(string userId, int exerciseId);

        public bool IsExists(string userId, int exerciseId);
    }
}
