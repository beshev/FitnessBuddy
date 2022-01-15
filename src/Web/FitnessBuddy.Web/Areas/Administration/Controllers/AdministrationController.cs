namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using FitnessBuddy.Common;
    using FitnessBuddy.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
