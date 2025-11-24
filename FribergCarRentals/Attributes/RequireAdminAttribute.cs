using FribergCarRentals.Mvc.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FribergCarRentals.Mvc.Attributes
{
    public class RequireAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UserSession userSession = context.HttpContext.RequestServices.GetRequiredService<UserSession>();
            if (!userSession.IsAdmin())
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
