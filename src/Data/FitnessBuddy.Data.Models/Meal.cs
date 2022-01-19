namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class Meal : BaseDeletableModel<int>
    {
        public Meal()
        {
            this.MealFoods = new HashSet<MealFood>();
        }

        [Required]
        [MaxLength(DataConstants.MealNameMaxLength)]
        public string Name { get; set; }

        public double TargetProtein { get; set; }

        public double TargetCarbs { get; set; }

        public double TargetFat { get; set; }

        [Required]
        public string ForUserId { get; set; }

        public ApplicationUser ForUser { get; set; }

        public ICollection<MealFood> MealFoods { get; set; }
    }
}
