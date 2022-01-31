﻿namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseDetailsModel : IMapFrom<Exercise>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string CategoryName { get; set; }

        public string EquipmentName { get; set; }

        public string AddedByUserUserName { get; set; }
    }
}