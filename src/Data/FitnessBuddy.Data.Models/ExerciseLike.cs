namespace FitnessBuddy.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Data.Common.Models;

    public class ExerciseLike : BaseModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }
    }
}
