namespace FitnessBuddy.Web.ViewModels.Trainings
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;

    public class TrainingInputModel
    {
        [Required]
        [StringLength(DataConstants.TrainingNameMaxLength, MinimumLength = DataConstants.TrainingNameMinLength)]
        public string Name { get; set; }

        public string UserId { get; set; }
    }
}
