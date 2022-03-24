namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.ExercisesLikes;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.ExercisesLikes;
    using Microsoft.AspNetCore.Mvc;

    public class ExercisesLikesController : ApiController
    {
        private readonly IExercisesLikesService exercisesLikesService;
        private readonly IExercisesService exercisesService;

        public ExercisesLikesController(
            IExercisesLikesService exercisesLikesService,
            IExercisesService exercisesService)
        {
            this.exercisesLikesService = exercisesLikesService;
            this.exercisesService = exercisesService;
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseLikeReturnModel>> Post(int exerciseId)
        {
            var userId = this.User.GetUserId();
            var isLike = this.exercisesLikesService.IsExists(userId, exerciseId);

            if (isLike)
            {
                await this.exercisesLikesService.UnLikeAsync(userId, exerciseId);
                isLike = false;
            }
            else if (await this.exercisesService.IsExistAsync(exerciseId))
            {
                await this.exercisesLikesService.LikeAsync(userId, exerciseId);
                isLike = true;
            }

            var returnModel = new ExerciseLikeReturnModel()
            {
                LikesCount = this.exercisesLikesService.GetExerciseLikesCount(exerciseId),
                IsUserLikeExercise = isLike,
            };

            return returnModel;
        }
    }
}
