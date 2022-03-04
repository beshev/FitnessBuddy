namespace FitnessBuddy.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class AllPostsViewModel : PagingViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
