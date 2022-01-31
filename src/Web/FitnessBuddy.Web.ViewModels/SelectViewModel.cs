namespace FitnessBuddy.Web.ViewModels
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class SelectViewModel :
        IMapFrom<ExerciseCategory>,
        IMapFrom<ExerciseEquipment>,
        IMapFrom<Meal>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
