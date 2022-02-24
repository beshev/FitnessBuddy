namespace FitnessBuddy.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Replies;

    public class PostDetailsViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<ReplyViewModel> Replies { get; set; }
    }
}
