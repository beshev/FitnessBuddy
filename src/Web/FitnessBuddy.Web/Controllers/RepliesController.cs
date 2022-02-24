namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Services.Data.Replies;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Replies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RepliesController : Controller
    {
        private readonly IRepliesService repliesService;

        public RepliesController(IRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        public IActionResult Add(int parentId)
        {
            var viewModel = this.repliesService.GetById<ReplyViewModel>(parentId);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReplyInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.RedirectToAction("Details", "Posts", new { id = model.PostId });
            }

            model.AuthorId = this.User.GetUserId();
            await this.repliesService.AddAsync(model);

            return this.RedirectToAction("Details", "Posts", new { Id = model.PostId });
        }

        public async Task<IActionResult> Delete(int replyId, int postId)
        {
            if (this.repliesService.IsExist(replyId) == false)
            {
                return this.NotFound();
            }

            if (this.repliesService.IsUserAuthor(replyId, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.repliesService.DeleteAsync(replyId);

            return this.RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
