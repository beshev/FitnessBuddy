namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

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

            return this.RedirectToAction(nameof(this.Categories));
        }
    }
}
