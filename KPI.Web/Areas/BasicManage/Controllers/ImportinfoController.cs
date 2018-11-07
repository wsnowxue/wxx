using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KPI.Application;
using KPI.Code;
using KPI.Application.SystemSecurity;
using System.Reflection;


namespace KPI.Web.Areas.BasicManage.Controllers
{
    public class ImportinfoController : ControllerBase
    {

        const string Namespace = "KPI.Application";

        /// <summary>
        /// 获取数据校验错误列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetErrorItemCollection(string filePath, string className, string[] keyValue)
        {
            ExcelImportBase importApp = CreateInstance(className);
            if (importApp == null) throw new Exception("创建反射实例失败！");
            var data = importApp.Scan(filePath, keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="className"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public ActionResult Import(string filePath, string className, string[] keyValue)
        {
            ExcelImportBase importApp = CreateInstance(className);
            var data = importApp.Scan(filePath, keyValue);
            if (data.Count > 0)
            {
                return Error("导入数据存在未处理的错误！");
            }
            else
            {
                importApp.Import(filePath, keyValue);
                return Success("数据导入成功！");
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="className"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public ActionResult DeleteRow(string filePath, string className, int rowNo, string keyValue)
        {
            ExcelImportBase importApp = CreateInstance(className);
            if (importApp == null) return Error("创建反射实例失败！");
            importApp.Delete(filePath, rowNo, keyValue);
            return Success("删除成功！");
        }

        /// <summary>
        /// 反射创建实例
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private ExcelImportBase CreateInstance(string className)
        {
            try
            {
                return (ExcelImportBase)(Assembly.Load(Namespace).CreateInstance(Namespace + className));
            }
            catch(Exception e)
            {
                return null;
            }
        }

    }
}