namespace FitnessBuddy.Web.ViewModels.Meals
{
    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MealFoodViewModel : IMapFrom<MealFood>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        public int MealId { get; set; }

        public string FoodName { get; set; }

        public double Protein { get; set; }

        public double Carbohydrates { get; set; }

        public double Fats { get; set; }

        public double Calories => ((this.Protein + this.Carbohydrates) * GlobalConstants.CaloriesForOneGramProteinAndCarbohydrates) + (this.Fats * GlobalConstants.CaloriesForOneGramFats);

        public double QuantityInGrams { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MealFood, MealFoodViewModel>()
                .ForMember(
                dest => dest.Protein,
                opt => opt.MapFrom(x => x.Food.ProteinIn100Grams * x.QuantityInGrams / 100))
                .ForMember(
                dest => dest.Carbohydrates,
                opt => opt.MapFrom(x => x.Food.CarbohydratesIn100Grams * x.QuantityInGrams / 100))
                .ForMember(
                dest => dest.Fats,
                opt => opt.MapFrom(x => x.Food.FatIn100Grams * x.QuantityInGrams / 100))
                .ForMember(
                dest => dest.FoodName,
                opt => opt.MapFrom(x => x.Food.FoodName.Name));
        }
    }
}
