using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Shared
{
    public abstract class AuthenticatedPageModel : PageModel
    {
        
        protected int? UserId { get; private set; }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);

            UserId = context.HttpContext.Session.GetInt32("UserId");
            if (!UserId.HasValue)
            {
                context.Result = new RedirectToPageResult("/Users/Login");
                return;
            }
        }
    }
}