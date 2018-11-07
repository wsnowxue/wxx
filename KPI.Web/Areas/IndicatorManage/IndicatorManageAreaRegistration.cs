using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Areas.IndicatorManage
{
    public class IndicatorManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IndicatorManage";
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