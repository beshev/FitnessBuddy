namespace FitnessBuddy.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class AllUsersViewModel : PagingViewModel
    {
        public IEnumerable<AdministrationUserViewModel> Users { get; set; }
    }
}
