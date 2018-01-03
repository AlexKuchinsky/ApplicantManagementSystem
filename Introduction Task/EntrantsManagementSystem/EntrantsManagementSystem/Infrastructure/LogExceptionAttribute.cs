using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using EntrantsManagementSystem.Logging;
using Microsoft.Practices.Unity;
using Ninject;
namespace EntrantsManagementSystem.Infrastructure
{
    public class LogExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        // Doesn't work
        //[InjectAttribute]
        public static ILogger Logger { get; set; } = new DatabaseLogger();

        public void OnException(ExceptionContext filterContext)
        {    
            Exception exception = filterContext.Exception;
            Logger.LogException(exception, DateTime.Now,"Was redirected to Error page");
            filterContext.Result = new ViewResult
            {
                ViewName = "ErrorPage",
                ViewData = new ViewDataDictionary<Exception>(exception)
            };
            filterContext.ExceptionHandled = true;
        }
    }
}