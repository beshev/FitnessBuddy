namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class PostCategory : BaseDeletableModel<int>
    {
        public PostCategory()
        {
            this.Posts = new HashSet<Post>();
        }

        [Required]
        [MaxLength(DataConstants.PostCategoryNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
