namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class AllUsersViewModel : PagingViewModel
    {
        public IEnumerable<AdministrationUserViewModel> Users { get; set; }
    }
}
