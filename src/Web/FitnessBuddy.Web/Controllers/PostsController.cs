namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        private readonly IPostCategoriesService postCategoriesService;
        private readonly IPostsService postsService;

        public PostsController(
            IPostCategoriesService postCategoriesService,
            IPostsService postsService)
        {
            this.postCategoriesService = postCategoriesService;
            this.postsService = postsService;
        }

        public async Task<IActionResult> Categories()
        {
            var viewModel = await this.postCategoriesService.GetAllAsync<PostCategoryViewModel>();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Category(int categoryId, int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var postsPerPage = 5;
            int count = await this.postsService.GetCountAsync(categoryId);
            int pagesCount = (int)Math.Ceiling((double)count / postsPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * postsPerPage;

            var posts = await this.postsService.GetAllAsync<PostViewModel>(categoryId, skip, postsPerPage);

            var viewModel = new AllPostsViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Posts = posts,
                CategoryName = await this.postCategoriesService.GetNameAsync(categoryId),
                CategoryId = categoryId,
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostInputModel model)
        {
            if (await this.postCategoriesService.IsExistAsync(model.CategoryId) == false)
            {
                this.ModelState.AddModelError("Category", "Invalid category!");
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            model.AuthorId = this.User.GetUserId();

            await this.postsService.CreateAsync(model);

            this.TempData[GlobalConstants.NameOfSuccess] = string.Format(GlobalConstants.SuccessMessage, GlobalConstants.NameOfPost);

            return this.RedirectToAction(nameof(this.Category), new { model.CategoryId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await this.postsService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            if (await this.postsService.IsUserAuthorAsync(id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            var viewModel = await this.postsService.GetByIdAsync<PostInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            if (await this.postsService.IsExistAsync(model.Id) == false)
            {
                return this.NotFound();
            }

            if (await this.postsService.IsUserAuthorAsync(model.Id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.postsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            if (await this.postsService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            var viewModel = await this.postsService.GetByIdAsync<PostDetailsViewModel>(id);

            await this.postsService.IncreaseViewsAsync(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (await this.postsService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            if (await this.postsService.IsUserAuthorAsync(id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.postsService.DeleteAsync(id);

            this.TempData[GlobalConstants.NameOfDelete] = string.Format(GlobalConstants.DeleteMessage, GlobalConstants.NameOfPost);

            return this.RedirectToAction(nameof(this.Categories));
        }
    }
}
