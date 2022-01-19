namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class FoodName : BaseDeletableModel<int>
    {
        public FoodName()
        {
            this.Foods = new HashSet<Food>();
        }

        [Required]
        [MaxLength(DataConstants.FoodNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Food> Foods { get; set; }
    }
}
