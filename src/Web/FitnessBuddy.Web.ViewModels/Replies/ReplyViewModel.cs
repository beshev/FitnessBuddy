namespace FitnessBuddy.Web.ViewModels.Replies
{
    using System;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ReplyViewModel : IMapFrom<Reply>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public int PostId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
