namespace FitnessBuddy.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class AllPostsViewModel : PagingViewModel
    {
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
