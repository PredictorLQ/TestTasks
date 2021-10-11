using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Picasso.Filter
{
    public class Ajax : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() == false)
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.Result = new NotFoundResult();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
    public static class AjaxControl
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request != null)
            {
                if (request.Headers != null)
                    return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return false;
        }
    }
}

