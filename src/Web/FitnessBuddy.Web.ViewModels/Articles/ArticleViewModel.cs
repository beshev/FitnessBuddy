namespace FitnessBuddy.Web.ViewModels.Articles
{
    using System;
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ArticleViewModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string CreatorUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public double AvarageRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleViewModel>()
                .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(x => $"/images/articles/{Path.GetFileName(x.ImageUrl)}"))
                .ForMember(
                dest => dest.AvarageRating,
                opt => opt.MapFrom(x => x.ArticleRatings.Any() ? x.ArticleRatings.Average(r => r.Rating) : 0))
                .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom(x => x.Content.Length > 300 ? $"{x.Content.Substring(0, 300)}..." : x.Content));
        }
    }
}
