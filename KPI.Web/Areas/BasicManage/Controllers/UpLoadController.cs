using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KPI.Application;
using KPI.Code;
using System.IO;
using System.Net.Http;



namespace KPI.Web.Areas.BasicManage.Controllers
{
    public class UpLoadController : ControllerBase
    {
        const string FilePath = @"/TempFile/";

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UpLoadFile(HttpRequestMessage request)
        {
            string serverPath = System.Web.HttpContext.Current.Server.MapPath(FilePath);
            if (!Directory.Exists(serverPath)) Directory.CreateDirectory(serverPath);
            List<UpLoadFileModel> list = new List<UpLoadFileModel>();
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                UpLoadFileModel model = new UpLoadFileModel();
                string timeStemp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileServName = serverPath + timeStemp + files[i].FileName.Substring(files[i].FileName.LastIndexOf('.'));
                files[i].SaveAs(fileServName);
                model.originalfilename = files[i].FileName;
                model.destfilename = fileServName;
                list.Add(model);
            }
            return Success("上传成功", list, list.Count);
        }

        public class UpLoadFileModel
        {
            public string originalfilename { get; set; }

            public string destfilename { get; set; }
        }
    }
}