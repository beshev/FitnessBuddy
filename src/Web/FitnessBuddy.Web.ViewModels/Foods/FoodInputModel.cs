namespace FitnessBuddy.Web.ViewModels.Foods
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Attributes;

    public class FoodInputModel : IMapTo<Food>, IMapFrom<Food>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DataConstants.FoodNameMinLength)]
        [MaxLength(DataConstants.FoodNameMaxLength)]
        [Display(Name = "Food name")]

        public string Name { get; set; }

        [Range(DataConstants.FoodNutritionsMinValue, DataConstants.FoodNutritionsMaxValue)]
        [Display(Name = "Protein for 100 grams")]
        public double ProteinIn100Grams { get; set; }

        [Range(DataConstants.FoodNutritionsMinValue, DataConstants.FoodNutritionsMaxValue)]
        [Display(Name = "Carbohydrates for 100 grams")]
        public double CarbohydratesIn100Grams { get; set; }

        [Range(DataConstants.FoodNutritionsMinValue, DataConstants.FoodNutritionsMaxValue)]
        [Display(Name = "Fat for 100 grams")]
        public double FatIn100Grams { get; set; }

        [NonNegativeNumber]
        [Display(Name = "Sodium in mg")]
        public double Sodium { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Food, FoodInputModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.FoodName.Name));
        }
    }
}
