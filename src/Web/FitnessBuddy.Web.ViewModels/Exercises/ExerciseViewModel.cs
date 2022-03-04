namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using System;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseViewModel : IMapFrom<Exercise>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string EquipmentName { get; set; }

        public string AddedByUserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
