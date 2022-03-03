namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PostsController : Controller
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

        public IActionResult Categories()
        {
            var viewModel = this.postCategoriesService.GetAll<PostCategoryViewModel>();

            return this.View(viewModel);
        }

        public IActionResult Category(int categoryId)
        {
            if (this.postCategoriesService.IsExist(categoryId) == false)
            {
                return this.NotFound();
            }

            var posts = this.postsService.GetPostsByCategory<PostViewModel>(categoryId);

            var viewModel = new CategoryPostsViewModel
            {
                Name = this.postCategoriesService.GetName(categoryId),
                Posts = posts,
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
            if (this.postCategoriesService.IsExist(model.CategoryId) == false)
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

        public IActionResult Edit(int id)
        {
            if (this.postsService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            if (this.postsService.IsUserAuthor(id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            var viewModel = this.postsService.GetById<PostInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            if (this.postsService.IsExist(model.Id) == false)
            {
                return this.NotFound();
            }

            if (this.postsService.IsUserAuthor(model.Id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.postsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }

        public IActionResult Details(int id)
        {
            if (this.postsService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            var viewModel = this.postsService.GetById<PostDetailsViewModel>(id);

            this.postsService.IncreaseViewsAsync(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.postsService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            if (this.postsService.IsUserAuthor(id, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.postsService.DeleteAsync(id);

            this.TempData[GlobalConstants.NameOfDelete] = string.Format(GlobalConstants.DeleteMessage, GlobalConstants.NameOfPost);

            return this.RedirectToAction(nameof(this.Categories));
        }
    }
}
