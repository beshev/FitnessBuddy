namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Administration.Foods;
    using FitnessBuddy.Web.ViewModels.Foods;
    using Microsoft.AspNetCore.Mvc;

    public class FoodsController : AdministrationController
    {
        private readonly IFoodsService foodsService;

        public FoodsController(
            IFoodsService foodsService)
        {
            this.foodsService = foodsService;
        }

        public IActionResult Index(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var foodPerPage = 15;
            int count = this.foodsService.GetCount(null);
            int pagesCount = (int)Math.Ceiling((double)count / foodPerPage);

            if (id > pagesCount)
            {
                return this.NotFound();
            }

            int skip = (id - 1) * foodPerPage;
            var foods = this.foodsService.GetAll<FoodViewModel>(null, null, skip, foodPerPage);

            var viewModel = new AllFoodsViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Foods = foods,
                ForAction = nameof(this.Index),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var viewModel = this.foodsService.GetByIdAsNoTracking<FoodDetailsViewModel>(id.Value);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.foodsService.AddAsync(this.User.GetUserId(), model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var viewModel = this.foodsService.GetByIdAsNoTracking<FoodInputModel>(id.Value);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FoodInputModel model)
        {
            if (this.foodsService.IsExist(model.Id) == false)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.foodsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var viewModel = this.foodsService.GetById(id.Value);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.foodsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
