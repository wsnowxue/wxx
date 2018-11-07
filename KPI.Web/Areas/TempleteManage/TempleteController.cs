using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Application.IndicatorManage;
using System.Web.Mvc;
using System.Globalization;
using KPI.Code;
using KPI.Application.TempleteManage;
using KPI.Domain.Entity.TempleteManage;
using KPI.Data;
using KPI.Domain.Entity.TaskManage;
using KPI.Application.TaskManage;
using KPI.Domain.ViewModel;

namespace KPI.Web.Areas.TempleteManage.Controllers
{
    public class TempleteController : ControllerBase
    {
        private TempleteApp templeteApp = new TempleteApp();
        private TempleteCheckApp templeteCheckApp = new TempleteCheckApp();
        private SelfTaskDetailApp selfTaskDetailApp = new SelfTaskDetailApp();

        private TempleteCompostionApp templeteCompostionApp = new TempleteCompostionApp();
        private SelfTaskDetailApp taskDetailApp = new SelfTaskDetailApp();



        #region 获取空的考核方案
        /// <summary>
        /// 获取系统已经维护的考核方案
        /// </summary>
        /// <param name="templete_name">考核方案名称  模糊搜索</param>
        /// <param name="templete_check_statue">审核状态</param>
        /// <param name="templete_type"></param>
        /// <param name="statue"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTempleteList(int? templete_type)
        {
            //首先获取第一个Templete的id
            var dataTemplete = templeteApp.GetList(templete_type);
            var data = new List<TempleteOverviewModel>();
            if (dataTemplete.Count == 0) return Success("操作成功。", data);
            string defaultId = dataTemplete.FirstOrDefault().id;
            data = templeteApp.GetTempleteOverviewByTempleteId(defaultId);

            return Success("操作成功。", data);

        }
        #endregion

        #region 获取系统默认考核方案
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="templete_type">0 集团考核分公司模板，1 分公司考核个人模板</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetDefaultTemplete(int? templete_type, string templete_name, int? statue, int? templete_check_statue, string creator_user_name, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            var data = templeteApp.GetTempleteOverview(templete_type, templete_name, statue, templete_check_statue, creator_user_name, pg);
            return Success("操作成功。", data, pg.records);

        }
        #endregion

        #region 获取需要自己审核的考核方案
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="templete_type">0 集团考核分公司模板，1 分公司考核个人模板</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetDefaultTempleteCheck(int? templete_type, string templete_name, int? statue, int? templete_check_statue, string checker, string creator_user_name, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            var data = templeteApp.GetCheckTempleteOverview(templete_type, templete_name, statue, templete_check_statue, checker, creator_user_name, pg);
            return Success("操作成功。", data, pg.records);

        }
        #endregion


        #region 获取某个考核方案详情
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="id">方案id</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTempleteDetail(string templete_id)
        {
            var data = templeteApp.GetTempleteOverviewByTempleteId(templete_id);
            return Success("操作成功。", data);

        }
        #endregion



        #region 新增考核方案
        /// <summary>
        /// 新增考核方案
        /// </summary>
        /// <param name="addTempleteList">考核方案list</param>
        /// <returns></returns>        

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult AddTemplete(string addTempleteList)
        {
            List<TempleteAddModel> templeteList = new List<TempleteAddModel>();
            FileLog.Debug("addTempleteList " + addTempleteList);
            templeteList = KPI.Code.Json.ToList<TempleteAddModel>(addTempleteList);
            if (templeteList == null || templeteList.Count == 0) { return Error("考核方案不能为空。"); }
            //先对考核方案的名称进行去重判断
            if (templeteApp.IsExists(templeteList[0].templete_name)) return Error("已存在相同名称的考核方案。");
            using (var db = new RepositoryBase().BeginTrans())
            {
                FileLog.Debug(addTempleteList);
                #region 新增考核方案
                TempleteEntity templeteEntity = new TempleteEntity();
                string templeteId = Common.GuId();
                templeteEntity.id = templeteId;
                templeteEntity.templete_name = templeteList[0].templete_name;
                templeteEntity.templete_type = templeteList[0].templete_type;
                templeteEntity.statue = 0;//1:启用 0：禁用
                templeteEntity.templete_check_statue = 0;//模板的初始化状态是null
                templeteEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                templeteEntity.create_time = DateTime.Now;
                //string templeteId = (templeteApp.SubmitFormEx(templeteEntity, string.Empty)).id;
                db.Insert<TempleteEntity>(templeteEntity);
                #endregion

                #region 新增待审核的模板
                TempleteCheckEntity templeteCheckEntity = new TempleteCheckEntity();
                string templeteCheckId = Common.GuId();
                templeteCheckEntity.id = templeteCheckId;
                templeteCheckEntity.templete_id = templeteId;
                templeteCheckEntity.check_sponser = OperatorProvider.Provider.GetCurrent().UserId;
                templeteCheckEntity.checker = templeteList[0].checker_id;//考虑审核方写谁？
                templeteCheckEntity.statue = 0;
                templeteCheckEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                templeteCheckEntity.create_time = DateTime.Now;
                //string templeteCheckId = (templeteCheckApp.SubmitFormEx(templeteCheckEntity, string.Empty)).id;
                db.Insert<TempleteCheckEntity>(templeteCheckEntity);
                #endregion

                #region 添加审核人的任务
                SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                selfTaskDetailEntity.id = Common.GuId();
                selfTaskDetailEntity.task_type = Convert.ToInt32(Constant.TaskType.待审核);
                selfTaskDetailEntity.person = templeteList[0].checker_id;
                selfTaskDetailEntity.task_statue = 1;//0 未激活  1待完成  2已完成
                selfTaskDetailEntity.task_object = templeteCheckId;
                selfTaskDetailEntity.statue = 0;//0未删除  1已删除
                selfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                selfTaskDetailEntity.create_time = DateTime.Now;
                //taskDetailApp.SubmitForm(selfTaskDetailEntity,String.Empty);
                db.Insert<SelfTaskDetailEntity>(selfTaskDetailEntity);
                #endregion

                #region 插入考核方案详细表
                TempleteCompostionEntity templeteCompostionEntity = null;

                for (int i = 0; i < templeteList.Count; i++)
                {
                    templeteCompostionEntity = new TempleteCompostionEntity();
                    templeteCompostionEntity.id = Common.GuId();
                    templeteCompostionEntity.dimension_id = templeteList[i].dimension_id;
                    templeteCompostionEntity.detail_id = templeteList[i].detail_id;
                    templeteCompostionEntity.base_score = templeteList[i].base_score;
                    templeteCompostionEntity.allow_beyond_limit = templeteList[i].allow_beyond_limit;
                    templeteCompostionEntity.templete_id = templeteId;
                    templeteCompostionEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                    templeteCompostionEntity.create_time = DateTime.Now;
                    db.Insert<TempleteCompostionEntity>(templeteCompostionEntity);
                    //templeteCompostionApp.SubmitForm(templeteCompostionEntity, String.Empty);
                }

                db.Commit();
            }
            #endregion
            return Success("操作成功。", null);
        }
        #endregion

        #region 更改审核方案
        /// <summary>
        /// 更改审核方案
        /// </summary>
        /// <param name="addTempleteList"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]

        public ActionResult UpdateTemplete(string templete_id, string updateTempleteList)
        {
            List<TempleteAddModel> templeteList = new List<TempleteAddModel>();


            templeteList = KPI.Code.Json.ToList<TempleteAddModel>(updateTempleteList);
            using (var db = new RepositoryBase().BeginTrans())
            {
                #region 将原来考核方案的状态更改为 未审核
                TempleteEntity templeteEntity = templeteApp.GetForm(templete_id);
                templeteEntity.templete_name = templeteList[0].templete_name;
                templeteEntity.templete_type = templeteList[0].templete_type;
                templeteEntity.statue = 0;//1:启用 0：禁用
                templeteEntity.templete_check_statue = 0;//模板的初始化状态是null
                templeteEntity.last_modify_time = DateTime.Now;
                //templeteApp.SubmitForm(templeteEntity, templete_id);
                db.Update<TempleteEntity>(templeteEntity);
                #endregion

                #region 删除原来考核方案的组成
                templeteCompostionApp.GetByTempleteId(templete_id).ForEach(t => { db.Delete<TempleteCompostionEntity>(t); });
                #endregion

                #region 插入考核方案详细表
                TempleteCompostionEntity templeteCompostionEntity = null;

                for (int i = 0; i < templeteList.Count; i++)
                {
                    templeteCompostionEntity = new TempleteCompostionEntity();
                    templeteCompostionEntity.id = Common.GuId();
                    templeteCompostionEntity.dimension_id = templeteList[i].dimension_id;
                    templeteCompostionEntity.detail_id = templeteList[i].detail_id;
                    templeteCompostionEntity.base_score = templeteList[i].base_score;
                    templeteCompostionEntity.allow_beyond_limit = templeteList[i].allow_beyond_limit;
                    templeteCompostionEntity.templete_id = templete_id;
                    templeteCompostionEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                    templeteCompostionEntity.create_time = DateTime.Now;
                    //templeteCompostionApp.SubmitForm(templeteCompostionEntity, String.Empty);
                    db.Insert<TempleteCompostionEntity>(templeteCompostionEntity);
                }
                #endregion



                #region 修改待审核的模板  通过置为空
                //获取这个方案的所有审核  如果是通过将将审核结果置为空
                TempleteCheckEntity templeteCheckEntity = templeteCheckApp.GetTempleteCheckByTempleteId(templete_id);

                templeteCheckEntity.check_sponser = templeteEntity.creator_user_id;
                templeteCheckEntity.checker = templeteList[0].checker_id;//考虑审核方写谁？
                templeteCheckEntity.statue = 0;
                templeteCheckEntity.last_modify_time = DateTime.Now;
                //templeteCheckApp.SubmitForm(templeteCheckEntity, templeteCheckEntity.id);
                db.Update<TempleteCheckEntity>(templeteCheckEntity);
                #endregion

                #region 添加审核人的任务  由于没有与审核人任务相关联的外键  所以进行添加  无法进行更新
                string templeteCheckId = templeteCheckEntity.id;
                //找到原来此任务  将任务改为待完成
                SelfTaskDetailEntity selfTaskDetailEntity  = selfTaskDetailApp.GetPersonalTaskByTaskObj(templeteCheckId, templeteList[0].checker_id);
                if (selfTaskDetailEntity == null) return Error("不存在对应的任务");
                selfTaskDetailEntity.task_statue = 1;//0 未激活  1待完成  2已完成
                selfTaskDetailEntity.last_modify_time = DateTime.Now;
                db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                #endregion



                db.Commit();
            }

            return Success("操作成功。", null);
        }
        #endregion

        public class TempleteAddModel
        {
            /// <summary>
            /// 方案名称
            /// </summary>
            public string templete_name { get; set; }
            /// <summary>
            /// 方案类型 集团考核分公司模板，1 分公司考核个人模板
            /// </summary>
            public int templete_type { get; set; }
            /// <summary>
            /// 维度id
            /// </summary>
            public string dimension_id { get; set; }
            /// <summary>
            /// 细则id
            /// </summary>
            public string detail_id { get; set; }
            /// <summary>
            /// 基础分值
            /// </summary>
            public float base_score { get; set; }
            /// <summary>
            /// 允许评分超限 0 不允许，1允许
            /// </summary>
            public int allow_beyond_limit { get; set; }
            /// <summary>
            /// 当前操作者id
            /// </summary>
            public string creator_id { get; set; }
            /// <summary>
            /// 考评人id
            /// </summary>
            public string checker_id { get; set; }
        }

        #region 启/停用系统已经维护的考核方案
        /// <summary>
        /// 启/停用系统已经维护的考核方案
        /// </summary>
        /// <param name="id">指标id 必填</param>
        /// <param name="statue">状态  0：停用，1：启用 必填</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ChangeTempletestatue(string templete_id, int statue)
        {
            if (String.IsNullOrEmpty(templete_id)) return Error("考核模板Id不存在！");
            TempleteEntity templeteEntity = new TempleteEntity();
            templeteEntity = templeteApp.GetTempleteById(templete_id);
            if (templeteEntity == null) return Error("考核模板不存在!");
            templeteEntity.statue = statue;
            templeteApp.SubmitForm(templeteEntity, templete_id);
            return Success("操作成功。");
        }
        #endregion
        

    }
}