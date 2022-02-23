namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class Reply : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.ReplyDescriptionMaxLength)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public virtual Reply Parent { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
