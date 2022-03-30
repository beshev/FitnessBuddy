namespace FitnessBuddy.Web.ViewModels.Users
{
    using System;
    using System.IO;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ShortUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }
    }
}
