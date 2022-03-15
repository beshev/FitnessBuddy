namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UserListViewModel : PagingViewModel
    {
        public IEnumerable<ShortUserViewModel> Users { get; set; }
    }
}
