namespace FitnessBuddy.Web.ViewModels.Replies
{
    using System.Collections.Generic;

    public class AllRepliesViewModel : PagingViewModel
    {
        public IEnumerable<ReplyViewModel> Replies { get; set; }
    }
}
