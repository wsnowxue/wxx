using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHttp = System.Web.Http;
using KPI.Domain.Entity.TaskManage;
using KPI.Application.TaskManage;
using System.Web.Mvc;
using System.Globalization;
using KPI.Code;
using KPI.Domain.ViewModel;

namespace KPI.Web.Areas.IndicatorManage.Controllers
{
    public class SelfTaskDetailController : ControllerBase
    {
        private SelfTaskDetailApp selfTaskDetailApp = new SelfTaskDetailApp();

        #region 新增任务信息
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult AddSelfTask(int task_type, string person, string task_object)
        {
            SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
            selfTaskDetailEntity.task_type = task_type;
            selfTaskDetailEntity.person = person;
            selfTaskDetailEntity.task_object = task_object;
            selfTaskDetailEntity.task_statue = 0;
            selfTaskDetailApp.SubmitForm(selfTaskDetailEntity, string.Empty);
            return Content(new AjaxResult { state = ResultType.success, message = "操作成功。" }.ToJson());
        }
        #endregion

        #region 更新任务完成状态
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ChangeSelfTaskStatue(string id, int task_statue)
        {
            SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
            selfTaskDetailEntity.task_statue = task_statue;
            selfTaskDetailApp.SubmitForm(selfTaskDetailEntity, id);
            return Content(new AjaxResult { state = ResultType.success, message = "操作成功。" }.ToJson());
        }
        #endregion

        #region 获取任务信息
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelfTask(string person, int task_type, int task_statue, string task_object, string pagination)
        {
            var data = selfTaskDetailApp.GetPersonalTask(person, task_type, task_statue, task_object, pagination);
            return Content(new AjaxResult { state = ResultType.success, message = "操作成功。", data = data }.ToJson());
        }
        #endregion

        #region 获取个人所有待完成任务信息
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelfAllTasks(string person)
        {
            List<TaskMode> list = new List<TaskMode>();
            list.Add(new TaskMode() { task_type = 1, task_count = 0 });
            list.Add(new TaskMode() { task_type = 2, task_count = 0 });
            list.Add(new TaskMode() { task_type = 3, task_count = 0 });
            list.Add(new TaskMode() { task_type = 4, task_count = 0 });
            var data = selfTaskDetailApp.GetSelfAllTasks(person);
            if (data != null)
            {
                data.ForEach((t) =>
                {
                    if (t.task_type == 1) list[0].task_count = t.task_count;
                    if (t.task_type == 2) list[1].task_count = t.task_count;
                    if (t.task_type == 3) list[2].task_count = t.task_count;
                    if (t.task_type == 4) list[3].task_count = t.task_count;
                });
            }
            
            //return Error("此用户不存在任务。");

            return Content(new AjaxResult { state = ResultType.success, message = "操作成功。", data = list, total = list.Count }.ToJson());
        }

        #endregion
    }
}