using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Application.IndicatorManage;
using System.Web.Mvc;
using KPI.Code;
using System.Globalization;


namespace KPI.Web.Areas.IndicatorManage.Controllers
{
    public class CheckMethodController : ControllerBase
    {
        private CheckMethodApp checkMethodApp = new CheckMethodApp();

        [HttpPost]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult AddCheckMethod(string check_method_name, string check_method_define, string check_method_proc)
        {
            if (checkMethodApp.IsExists(check_method_name)) return Error("已存在相同条目。");
            CheckMethodEntity entity = new CheckMethodEntity();
            entity.check_method_name = check_method_name;
            entity.check_method_define = check_method_define;
            entity.check_method_statue = 0;
            entity.check_method_proc = check_method_proc;
            checkMethodApp.SubmitForm(entity, string.Empty);
            return Success("操作成功。");
        }

        [HttpGet]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult GetCheckMethod(string check_method_name, string check_method_define, int? check_method_statue, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            //默认根据创建时间进行分页排序
            pg.sidx = "create_time";
            pg.sord = "desc";
            var data = new List<CheckMethodEntity>();
            data = checkMethodApp.GetItemList(check_method_name, check_method_define, check_method_statue, pg);
            return Success("操作成功。", data, pg.records);
        }

        [HttpPost]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult ChangeCheckMethodStatue(string check_method_id, int check_method_statue)
        {
            CheckMethodEntity checkMethodEntity = checkMethodApp.GetForm(check_method_id);
            checkMethodEntity.check_method_statue = check_method_statue;
            checkMethodApp.SubmitForm(checkMethodEntity, check_method_id);
            return Success("操作成功。");
        }

    }
}