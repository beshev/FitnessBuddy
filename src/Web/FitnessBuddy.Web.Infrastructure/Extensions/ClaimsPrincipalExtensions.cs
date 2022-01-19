namespace FitnessBuddy.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Email).Value;
        }
    }
}
