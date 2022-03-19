namespace FitnessBuddy.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Data.Common.Models;

    public class ArticleRating : BaseModel<int>
    {
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public double Rating { get; set; }
    }
}
