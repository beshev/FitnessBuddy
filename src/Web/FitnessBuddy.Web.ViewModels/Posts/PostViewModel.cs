namespace FitnessBuddy.Web.ViewModels.Posts
{
    using System;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string AuthorUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RepliesCount { get; set; }

        public int Views { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(x => x.Description.Length > 100 ? $"{x.Description.Substring(0, 100)}..." : x.Description));
        }
    }
}
