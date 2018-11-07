using System.Web.Mvc;

namespace KPI.Web.Areas.AssessmentManage
{
    public class AssessmentManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AssessmentManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "KPI.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
