namespace FitnessBuddy.Web.ViewModels
{
    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public int FirstPageNumber => 1;

        public int LastPageNumber => this.PagesCount;

        public int PagesCount { get; set; }

        public string ForAction { get; init; }

        public string ForController { get; init; }

        public string Search { get; set; }
    }
}
