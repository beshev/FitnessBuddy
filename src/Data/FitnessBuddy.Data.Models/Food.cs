namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class Food : BaseDeletableModel<int>
    {
        public Food()
        {
            this.FoodMeals = new HashSet<MealFood>();
            this.FavoriteUsersFood = new HashSet<ApplicationUser>();
        }

        public int FoodNameId { get; set; }

        public virtual FoodName FoodName { get; set; }

        [Required]
        [MaxLength(DataConstants.FoodDescriptionMaxLength)]
        public string Description { get; set; }

        public double ProteinIn100Grams { get; set; }

        public double CarbohydratesIn100Grams { get; set; }

        public double FatIn100Grams { get; set; }

        public double Sodium { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }

        public virtual ICollection<MealFood> FoodMeals { get; set; }

        public virtual ICollection<ApplicationUser> FavoriteUsersFood { get; set; }
    }
}
