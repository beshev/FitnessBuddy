namespace FitnessBuddy.Web.ViewModels.Articles
{
    using System;
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ArticleDetailsModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string CreatorUsername { get; set; }

        public string CreatorId { get; set; }

        public bool IsCreator { get; set; }

        public DateTime CreatedOn { get; set; }

        public double AvgRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleDetailsModel>()
                .ForMember(
                dest => dest.AvgRating,
                opt => opt.MapFrom(x => x.ArticleRatings.Any() ? x.ArticleRatings.Average(x => x.Rating) : 0));
        }
    }
}
