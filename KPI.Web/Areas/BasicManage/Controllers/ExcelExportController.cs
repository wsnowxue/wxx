using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KPI.Application;
using KPI.Code;



namespace KPI.Web.Areas.BasicManage.Controllers
{
    public class ExcelExportController : ControllerBase
    {
        public void ExecuteExportExcel(string dataJson, string templdateName, string newFileName)
        {
            List<TemplateMode> list = dataJson.ToList<TemplateMode>();
            ExcelHelper.ExcelDownload(list, templdateName, newFileName);
        }
    }
}