namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseDetailsModel : IMapFrom<Exercise>, IMapFrom<ExerciseLike>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string CategoryName { get; set; }

        public string EquipmentName { get; set; }

        public int ExerciseLikesCount { get; set; }

        public bool IsCreator { get; set; }

        public bool IsUserLikeExercise { get; set; }
    }
}
