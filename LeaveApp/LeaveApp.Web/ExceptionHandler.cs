using System;
using System.Web.Mvc;

namespace LeaveApp.Web
{
    public class ExceptionHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            string action = filterContext.RouteData.Values["action"].ToString();
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            //filterContext.Result = new RedirectToAction("Error", "InternalError");
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }
    }
}