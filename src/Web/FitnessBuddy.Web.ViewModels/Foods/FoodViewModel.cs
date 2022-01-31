namespace FitnessBuddy.Web.ViewModels.Foods
{
    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class FoodViewModel : IMapFrom<Food>, IMapTo<Food>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FoodName { get; set; }

        public double ProteinIn100Grams { get; set; }

        public double CarbohydratesIn100Grams { get; set; }

        public double FatIn100Grams { get; set; }

        public double Sodium { get; set; }

        public string FoodCalories
            => (((this.ProteinIn100Grams + this.CarbohydratesIn100Grams) * GlobalConstants.CaloriesForOneGramProteinAndCarbohydrates) + (this.FatIn100Grams * GlobalConstants.CaloriesForOneGramFats)).ToString("F2");

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Food, FoodViewModel>()
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(x => x.FoodName.Name));
        }
    }
}
