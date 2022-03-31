namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Replies;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Replies;
    using Microsoft.AspNetCore.Mvc;

    public class RepliesController : BaseController
    {
        private readonly IRepliesService repliesService;

        public RepliesController(IRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        public async Task<IActionResult> Add(int parentId)
        {
            var viewModel = await this.repliesService.GetByIdAsync<ReplyViewModel>(parentId);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReplyInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                model.AuthorId = this.User.GetUserId();
                await this.repliesService.AddAsync(model);
            }

            return this.RedirectToAction(GlobalConstants.NameOfDetails, GlobalConstants.NameOfPosts, new { id = model.PostId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await this.repliesService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            if (await this.repliesService.IsUserAuthorAsync(id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            var viewModel = await this.repliesService.GetByIdAsync<ReplyEditInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReplyEditInputModel model)
        {
            if (await this.repliesService.IsExistAsync(model.Id) == false)
            {
                return this.NotFound();
            }

            if (await this.repliesService.IsUserAuthorAsync(model.Id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.repliesService.EditAsync(model);

            var postId = await this.repliesService.GetReplyPostIdAsync(model.Id);

            return this.RedirectToAction("Details", "Posts", new { Id = postId });
        }

        public async Task<IActionResult> Delete(int replyId, int postId)
        {
            if (await this.repliesService.IsExistAsync(replyId) == false)
            {
                return this.NotFound();
            }

            if (await this.repliesService.IsUserAuthorAsync(replyId, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.repliesService.DeleteAsync(replyId);

            return this.RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
