namespace FitnessBuddy.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Data.Common.Models;

    public class UserFollower : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string FollowerId { get; set; }

        public virtual ApplicationUser Follower { get; set; }
    }
}
