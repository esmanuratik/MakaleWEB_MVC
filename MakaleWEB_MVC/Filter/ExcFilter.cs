using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakaleWEB_MVC.Filter
{
    public class ExcFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["hatalar"] = filterContext.Exception;
            filterContext.ExceptionHandled = true;//true olmasının sebebi ben yönlendiricem anlamında
            filterContext.Result = new RedirectResult("/Home/Hatalıİslem");

        }
    }
}