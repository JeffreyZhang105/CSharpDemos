using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using WebGrease;
using LogManager = log4net.LogManager;

namespace WebApiDemos.Filters
{
    public class MyweatherExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = LogManager.GetLogger(this.GetType());
            logger.Error(context.Exception.StackTrace);
        }
    }
}