namespace FitnessBuddy.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class CategoryPostsViewModel
    {
        public string Name { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
