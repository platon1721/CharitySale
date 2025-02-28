using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Shared
{
    public abstract class AdminPageModel : PageModel
    {
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);

            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                context.Result = new RedirectToPageResult("/Users/Login");
                return;
            }

            if (HttpContext.Session.GetString("UserIsAdmin") != "True")
            {
                context.Result = new RedirectToPageResult("/Shared/AccessDenied");
                return;
            }
        }
    }
}