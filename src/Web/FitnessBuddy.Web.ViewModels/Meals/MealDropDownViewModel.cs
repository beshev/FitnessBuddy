namespace FitnessBuddy.Web.ViewModels.Meals
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MealDropDownViewModel : IMapFrom<Meal>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
