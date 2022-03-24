namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AdministrationController
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var postsPerPage = 15;
            int count = await this.postsService.GetCountAsync();
            int pagesCount = (int)Math.Ceiling((double)count / postsPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * postsPerPage;

            var posts = await this.postsService.GetAllAsync<PostViewModel>(null, skip, postsPerPage);

            var viewModel = new AllPostsViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Posts = posts,
                ForAction = nameof(this.Index),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.postsService.GetByIdAsync<PostDetailsViewModel>(id.Value);

            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            model.AuthorId = this.User.GetUserId();
            await this.postsService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.postsService.GetByIdAsync<PostInputModel>(id.Value);

            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostInputModel model)
        {
            if (await this.postsService.IsExistAsync(model.Id) == false)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.postsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.postsService.GetByIdAsync<PostDetailsViewModel>(id.Value);

            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.postsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
