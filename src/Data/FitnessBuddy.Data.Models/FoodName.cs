namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;

    using FitnessBuddy.Data.Common.Models;

    public class FoodName : BaseDeletableModel<int>
    {
        public FoodName()
        {
            this.Foods = new HashSet<Food>();
        }

        public string Name { get; set; }

        public ICollection<Food> Foods { get; set; }
    }
}
