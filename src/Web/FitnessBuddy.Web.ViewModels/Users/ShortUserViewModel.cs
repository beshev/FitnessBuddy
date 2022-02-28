namespace FitnessBuddy.Web.ViewModels.Users
{
    using System;
    using System.IO;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ShortUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, ShortUserViewModel>()
                .ForMember(
                dest => dest.ProfilePicture,
                opt => opt.MapFrom(x => $"/images/profileimages/{Path.GetFileName(x.ProfilePicture)}"));
        }
    }
}
