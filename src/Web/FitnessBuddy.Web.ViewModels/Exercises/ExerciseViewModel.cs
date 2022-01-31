namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseViewModel : IMapFrom<Exercise>
    {
        public string Name { get; set; }

        public string Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string EquipmentName { get; set; }
    }
}
