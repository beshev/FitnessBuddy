namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Foods;
    using Microsoft.AspNetCore.Mvc;

    public class FoodsController : Controller
    {
        private readonly IFoodsService foodsService;

        public FoodsController(IFoodsService foodsService)
        {
            this.foodsService = foodsService;
        }

        public IActionResult All()
        {
            return this.View();
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(FoodViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.foodsService.AddAsync(userId, model);

            return this.Redirect("/");

            // return this.RedirectToAction(nameof(this.All));
        }
    }
}
