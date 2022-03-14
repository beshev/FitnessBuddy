namespace FitnessBuddy.Web.ViewModels.Restrictions
{
    using System;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class UserBanViewModel : IMapFrom<ApplicationUser>
    {
        public string BanReason { get; set; }

        public DateTime BannedOn { get; set; }
    }
}
