namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Replies;
    using FitnessBuddy.Web.ViewModels.Replies;
    using Microsoft.AspNetCore.Mvc;

    public class RepliesController : AdministrationController
    {
        private readonly IRepliesService repliesService;

        public RepliesController(IRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var repliesPerPage = 15;
            int count = await this.repliesService.GetCountAsync();
            int pagesCount = (int)Math.Ceiling((double)count / repliesPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * repliesPerPage;

            var replies = await this.repliesService.GetAllAsync<ReplyViewModel>(skip, repliesPerPage);

            var viewModel = new AllRepliesViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Replies = replies,
                ForAction = nameof(this.Index),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var reply = await this.repliesService.GetByIdAsync<ReplyViewModel>(id.Value);

            if (reply == null)
            {
                return this.NotFound();
            }

            return this.View(reply);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.repliesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
