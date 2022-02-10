﻿namespace FitnessBuddy.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class Article : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.ArticleTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DataConstants.ArticleContentMaxLength)]
        public string Content { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual ArticleCategory Category { get; set; }

        [Required]
        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }
    }
}