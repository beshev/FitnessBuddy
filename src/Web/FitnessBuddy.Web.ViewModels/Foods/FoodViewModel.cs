namespace FitnessBuddy.Web.ViewModels.Foods
{
    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class FoodViewModel : IMapFrom<Food>, IMapTo<Food>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FoodName { get; set; }

        public string Description { get; set; }

        public double ProteinIn100Grams { get; set; }

        public double CarbohydratesIn100Grams { get; set; }

        public double FatIn100Grams { get; set; }

        public double Sodium { get; set; }

        public string FoodCalories
            => (((this.ProteinIn100Grams + this.CarbohydratesIn100Grams) * 4) + (this.FatIn100Grams * 9)).ToString("F2");

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Food, FoodViewModel>()
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(x => x.FoodName.Name));
        }
    }
}
