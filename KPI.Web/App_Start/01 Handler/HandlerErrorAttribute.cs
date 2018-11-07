using KPI.Code;
using System.Web.Mvc;

namespace KPI.Web
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;

            if (context.Exception.GetType().Name == "NoLoginException")
            {
                context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.nologin, message = context.Exception.Message }.ToJson() };
            }
            else
            {
                context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error, message = context.Exception.Message }.ToJson() };
            }
            WriteLog(context);
        }
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            var log = LogFactory.GetLogger("ErrorLogger");
            log.Error("源：" + context.Controller.GetType().Name + ",错误信息：" + context.Exception);
        }
    }
}