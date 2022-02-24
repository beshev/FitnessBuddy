namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Web.ViewModels.Replies;
    using Microsoft.AspNetCore.Mvc;

    public class AddReplyViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int postId, int? parentId = null)
        {
            var viewModel = new ReplyInputModel
            {
                PostId = postId,
                ParentId = parentId,
            };

            return this.View(viewModel);
        }
    }
}
