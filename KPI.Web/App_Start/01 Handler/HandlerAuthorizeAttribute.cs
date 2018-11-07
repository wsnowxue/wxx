using KPI.Application.SystemManage;
using KPI.Code;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web
{
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
        public HandlerAuthorizeAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                return;
            }
            if (Ignore == false)
            {
                return;
            }
            if (!this.ActionAuthorize(filterContext))
            {
                filterContext.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error, message = "很抱歉！您的权限不足，访问被拒绝！" }.ToJson() };
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var roleId = operatorProvider.RoleId;
            var moduleId = WebHelper.GetCookie("KPI_currentmoduleid");
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            return new RoleAuthorizeApp().ActionValidate(roleId, moduleId, action);
        }
    }
}