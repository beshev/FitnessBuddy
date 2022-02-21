namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseInputModel : IMapFrom<Exercise>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.ExerciseNameMaxLength)]
        [MinLength(DataConstants.ExerciseNameMinLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Instructions")]
        public string Description { get; set; }

        [Range(DataConstants.ExerciseDifficultyMinValue, DataConstants.ExerciseDifficultyMaxValue)]
        public ExerciseDifficulty Difficulty { get; set; }

        [Url]
        [Display(Name = "Image link")]
        public string ImageUrl { get; set; }

        [Url]
        [Display(Name = "YouTube link")]
        public string VideoUrl { get; set; }

        public int CategoryId { get; set; }

        public int EquipmentId { get; set; }
    }
}
