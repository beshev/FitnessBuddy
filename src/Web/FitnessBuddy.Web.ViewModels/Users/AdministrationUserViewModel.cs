namespace FitnessBuddy.Web.ViewModels.Users
{
    using System;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;
    using FitnessBuddy.Services.Mapping;

    public class AdministrationUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public GenderType Gender { get; set; }

        public DateTime? BannedOn { get; set; }

        public bool IsBanned { get; set; }
    }
}
