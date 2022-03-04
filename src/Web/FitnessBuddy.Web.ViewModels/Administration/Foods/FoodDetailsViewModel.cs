namespace FitnessBuddy.Web.ViewModels.Administration.Foods
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Foods;

    public class FoodDetailsViewModel : IMapFrom<Food>, IMapTo<Food>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Food name")]
        public string FoodName { get; set; }

        [Display(Name = "Protein per 100 grams")]
        public double ProteinIn100Grams { get; set; }

        [Display(Name = "Carbohydrates per 100 grams")]
        public double CarbohydratesIn100Grams { get; set; }

        [Display(Name = "Fat per 100 grams")]
        public double FatIn100Grams { get; set; }

        [Display(Name = "Sodium per 100 grams")]
        public double Sodium { get; set; }

        [Display(Name = "Added by")]
        public string AddedByUserUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FoodCalories
            => (((this.ProteinIn100Grams + this.CarbohydratesIn100Grams) * GlobalConstants.CaloriesForOneGramProteinAndCarbohydrates) + (this.FatIn100Grams * GlobalConstants.CaloriesForOneGramFats)).ToString("F2");

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Food, FoodViewModel>()
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(x => x.FoodName.Name));
        }
    }
}
