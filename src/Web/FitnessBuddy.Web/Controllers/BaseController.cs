namespace FitnessBuddy.Web.Controllers
{
    using FitnessBuddy.Web.Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [TypeFilter(typeof(RestrictBannedUsersAttribute))]
    public class BaseController : Controller
    {
    }
}
