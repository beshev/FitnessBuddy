namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using FitnessBuddy.Data.Models.Enums;

    public class ExerciseInputModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ExerciseDifficulty Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public int CategoryId { get; set; }

        public int EquipmentId { get; set; }
    }
}
