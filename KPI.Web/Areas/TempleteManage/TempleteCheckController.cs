using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Application.IndicatorManage;
using System.Web.Mvc;
using System.Globalization;
using KPI.Code;
using KPI.Domain.Entity.TempleteManage;
using KPI.Domain.Entity.TaskManage;
using KPI.Application.TaskManage;
using KPI.Application.TempleteManage;
using KPI.Data;

namespace KPI.Web.Areas.TempleteManage.Controllers
{
    public class TempleteCheckController : ControllerBase
    {
        private TempleteCheckApp templeteCheckApp = new TempleteCheckApp();
        private TempleteApp templeteApp = new TempleteApp();
        private SelfTaskDetailApp selfTaskDetailApp = new SelfTaskDetailApp();



        #region 提交模板审核结果以及意见
        /// <summary>
        /// 提交模板审核结果以及意见
        /// </summary>
        /// <param name="id">考核方案id</param>
        /// <param name="check_result">审核结果  0 退回，1 通过</param>
        /// <param name="check_suggest">审核意见</param>
        /// <param name="checker">当前操作者id  退回时为必填项</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult PostTempleteCheckResult(string check_templete_id, int check_result, string check_suggest, string checker)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                #region 修改审核t_kpi_templete_check表的状态
                TempleteCheckEntity templeteCheckEntity = templeteCheckApp.GetTempleteCheckByTempleteCheckId(check_templete_id);
                if (templeteCheckEntity == null) { return Error("不存在该审核条目。"); }
                templeteCheckEntity.check_result = check_result;//0 不通过，1 通过
                templeteCheckEntity.check_suggest = check_suggest;//如果未通过则必须填写未通过的原因
                templeteCheckEntity.checker = checker;
                templeteCheckEntity.last_modify_time = DateTime.Now;
                //templeteCheckEntity = templeteCheckApp.SubmitFormEx(templeteCheckEntity, check_templete_id);
                db.Update<TempleteCheckEntity>(templeteCheckEntity);
                #endregion

                #region 修改审核t_kpi_templete表的状态

                TempleteEntity templeteEntity = templeteApp.GetForm(templeteCheckEntity.templete_id);
                if (templeteEntity == null) { return Error("不存在该考核模板条目。"); }
                templeteEntity.templete_check_statue = check_result;//0 待审核，1 通过，2退回
                //方案申核通過  默认启用
                templeteEntity.statue = check_result == 1 ? 1 : 0;//0 禁用，1 启用
                templeteEntity.last_modify_time = DateTime.Now;
                //templeteApp.SubmitForm(templeteEntity, templeteCheckEntity.templete_id);
                db.Update<TempleteEntity>(templeteEntity);
                #endregion


                #region 更新我的任务 对应的任务状态
                SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                //通过对应task_object获取
                selfTaskDetailEntity = selfTaskDetailApp.GetPersonalTaskByTaskObj(check_templete_id,checker);
                if (selfTaskDetailEntity == null) { return Error("“我的任务”不存在该条目。"); }
                selfTaskDetailEntity.task_statue = 2;//0 未激活  1待完成  2已完成
                selfTaskDetailEntity.task_object = check_templete_id;
                selfTaskDetailEntity.last_modify_time = DateTime.Now;
                //selfTaskDetailApp.SubmitForm(selfTaskDetailEntity, selfTaskDetailEntity.id);
                db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                db.Commit();
                #endregion
            }
            #endregion
            return Success("操作成功。");

        }
    }
}