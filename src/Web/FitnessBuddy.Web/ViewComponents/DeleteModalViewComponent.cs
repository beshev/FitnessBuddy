namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class DeleteModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int itemId, string controllerName, string itemName)
        {
            var viewModel = new DeleteModalViewModel
            {
                ItemId = itemId,
                ItemName = itemName,
                ContorollerName = controllerName,
                ModalId = $"deleteModal{itemId}",
            };

            return this.View(viewModel);
        }
    }
}
