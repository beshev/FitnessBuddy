namespace FitnessBuddy.Web.ViewModels.Meals
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MealFoodInputModel : IMapFrom<Food>, IHaveCustomMappings
    {
        public int FoodId { get; set; }

        public string FoodName { get; set; }

        public string ImageUrl { get; set; }

        public int MealId { get; set; }

        [Range(
            DataConstants.MealFoodQuantityMinValue,
            DataConstants.MealFoodQuantityMaxValue,
            ErrorMessage = "Quantity must be between {1} and {2}.")]
        [Display(Name = "Quantity in grams")]
        public double QuantityInGrams { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Food, MealFoodInputModel>()
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(x => x.FoodName.Name))
                .ForMember(dest => dest.FoodId, opt => opt.MapFrom(x => x.Id));
        }
    }
}
