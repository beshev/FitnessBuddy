namespace FitnessBuddy.Web.ViewModels.Articles
{
    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using System;

    public class ArticleDetailsModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleDetailsModel>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(x => x.AddedByUser.UserName));
        }
    }
}
