namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Foods;
    using Microsoft.AspNetCore.Authorization;
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
            var viewModel = this.foodsService.GetAll();

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(FoodViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.foodsService.AddAsync(userId, model);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var food = this.foodsService.GetFoodById(id);

            var viewModel = AutoMapperConfig.MapperInstance.Map<Food, FoodViewModel>(food);

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(FoodViewModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.foodsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await this.foodsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
