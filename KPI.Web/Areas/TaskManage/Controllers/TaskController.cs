using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebHttp = System.Web.Http;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Application.IndicatorManage;
using System.Web.Mvc;
using System.Globalization;
using KPI.Code;
using KPI.Application.TaskManage;
using KPI.Domain.Entity.TaskManage;
using KPI.Web.Areas.BasicManage.Controllers;
using System.Data;
using KPI.Domain.Entity.SystemManage;
using KPI.Application.SystemManage;
using KPI.Data;
using System.Text;

namespace KPI.Web.Areas.IndicatorManage.Controllers
{
    public class TaskController : ControllerBase
    {
        //中安金控所属机构在系统机构表中的主键
        private readonly string BlocId = Configs.GetValue("BlocId");
        //分公司考核管理员所属机构在系统机构表中的主键
        private readonly string CompanyId = Configs.GetValue("CompanyId");
        

        //客户经理角色的主键
        private readonly string CustomerManagerRoleId = Configs.GetValue("CustomerManagerRoleId");

        //经理室角色的主键
        private readonly string ManagerFamilyRoleId = Configs.GetValue("ManagerFamilyRoleId");
        

        private TaskApp taskApp = new TaskApp();
        private SelfTaskDetailApp selfTaskDetailApp = new SelfTaskDetailApp();
        private YearlyTaskDetailApp yearlyTaskDetailApp = new YearlyTaskDetailApp();
        private BranchCooperateBankTaskDetailApp branchCoopertionBankTaskDetailApp = new BranchCooperateBankTaskDetailApp();
        private BranchFinancialProductTaskDetailApp branchFinancialProductTaskDetailApp = new BranchFinancialProductTaskDetailApp();
        private OrganizeApp organizeApp = new OrganizeApp();
        private UserApp userApp = new UserApp();


        #region 获取历史任务分发记录
        /// <summary>
        /// 获取历史任务分发记录
        /// </summary>
        /// <param name="task_name">任务名称</param>
        /// <param name="task_statue">任务分发状态</param>
        /// <param name="task_type">任务类型 分公司 个人</param>
        /// <param name="user_id">当前操作者id</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTask(string task_name, int? task_statue, int? task_type, string user_id, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            var data = taskApp.GetTaskListOverview(pg, task_name, task_statue, task_type, user_id);
            return Success("操作成功。", data, pg.records);
        }
        #endregion

        #region 删除历史任务分发记录
        /// <summary>
        /// 删除历史任务分发记录
        /// </summary>
        /// <param name="task_name">任务名称</param>
        /// <param name="task_statue">任务分发状态</param>
        /// <param name="user_id">当前操作者id</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteTask(string task_id)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                #region 先删除主任务
                //查找此任务是否存在

                TaskDistributionEntity taskDistributionEntity = taskApp.GetForm(task_id);
                if (taskDistributionEntity == null) return Error("数据库中找不到此任务！");
                taskDistributionEntity.statue = 1;//0 未删除，1 已删除
                db.Update<TaskDistributionEntity>(taskDistributionEntity);
                #endregion
                //再删除分发的任务

                List<SelfTaskDetailEntity> selfTaskDetailList = selfTaskDetailApp.GetSelfTaskByTaskObj(task_id);
                if (selfTaskDetailList.Count > 0)
                {
                    for (int i = 0; i < selfTaskDetailList.Count; i++)
                    {
                        SelfTaskDetailEntity selfTaskDetailEntity = selfTaskDetailList[i];
                        selfTaskDetailEntity.statue = 1;//删除
                        db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                    }

                }
                db.Commit();
            }
            return Success("操作成功。");
        }
        #endregion

        #region 分发任务
        /// <summary>
        /// 对任务进行分发
        /// </summary>
        /// <param name="task_name">任务名称</param>
        /// <param name="task_statue">任务分发状态</param>
        /// <param name="user_id">当前操作者id</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UpdateTaskStatue(string task_id)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                TaskDistributionEntity taskDistributionEntity = new TaskDistributionEntity();
                taskDistributionEntity = taskApp.GetTaskById(task_id);
                if (taskDistributionEntity == null) { return Error("不存在该任务"); }
                taskDistributionEntity.task_distribute_statue = 1;//0:待分发  1：已分发
                db.Update<TaskDistributionEntity>(taskDistributionEntity);

                List<SelfTaskDetailEntity> selfTaskDetailList = selfTaskDetailApp.GetSelfTaskByTaskObj(task_id);
                if (selfTaskDetailList.Count > 0)
                {
                    for (int i = 0; i < selfTaskDetailList.Count; i++)
                    {
                        SelfTaskDetailEntity selfTaskDetailEntity = selfTaskDetailList[i];
                        selfTaskDetailEntity.task_statue = 2;//0 未激活  1待完成  2已完成
                        db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                    }

                }
                db.Commit();
            }
                

            return Success("操作成功。");

        }
        #endregion

        #region 创建任务
        /// <summary>
        /// 创建并且分发计划任务
        /// </summary>
        /// <param name="indicator_name">指标名称 模糊搜索</param>
        /// <param name="indicator_Define">指标定义 模糊搜索</param>
        /// <param name="count_method">统计方式 模糊搜索</param>
        /// <param name="start_index">起始索引</param>
        /// <param name="total">查询条数</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult addTask(string task_name, DateTime? start_time, DateTime? end_time, int task_type, string file1, string file2, string file3, string creator_user_id)
        {
            if (task_type == 0 && (String.IsNullOrEmpty(file1) || String.IsNullOrEmpty(file2) || String.IsNullOrEmpty(file3)))
            {
                return Error("请依次上传全年任务/金融产品/合作银行对应的execl文件，目前文件数目小于3！");
            }
            //限制任务名称不能重复
            if (taskApp.IsExists(task_name)) return Error("已存在相同名称的任务。");
            using (var db = new RepositoryBase().BeginTrans())
            {
                #region 添加分发任务 状态为：未分发
                TaskDistributionEntity taskDistributionEntity = new TaskDistributionEntity();
                string taskId = Common.GuId();
                taskDistributionEntity.id = taskId;
                taskDistributionEntity.task_name = task_name;
                taskDistributionEntity.assessment_start_time = start_time;
                taskDistributionEntity.assessment_end_time = end_time;
                taskDistributionEntity.task_sponsor = creator_user_id;//由user_id得到用户姓名
                taskDistributionEntity.task_distribute_statue = 0;//0待分发，1已分发
                taskDistributionEntity.task_type = task_type;
                taskDistributionEntity.statue = 0;
                taskDistributionEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                taskDistributionEntity.create_time = DateTime.Now;
                db.Insert<TaskDistributionEntity>(taskDistributionEntity);
                #endregion


                ///根据任务类型判断是个人任务还是公司任务
                ///个人任务1：导入一个Excel
                ///公司任务0：同时导入三个Excel

                string[] arr = new string[2];
                arr[0] = taskId;
                arr[1] = task_type.ToString();
                //ImportinfoController importController = new BasicManage.Controllers.ImportinfoController();
                StringBuilder errorBuilder = new StringBuilder();

                if (!string.IsNullOrEmpty(file1))
                {
                    //导入考核数据
                    YearlyTaskDetailImportApp yearlyTaskDetailImportApp = new YearlyTaskDetailImportApp(db);
                    var data = yearlyTaskDetailImportApp.Scan(file1, arr);
                    if (data.Count > 0)
                    {
                        return Error(data[0].ErrorReson);
                    }
                    else yearlyTaskDetailImportApp.ImportEx(file1, arr);


                    //importController.Import(file1, ".TaskManage.YearlyTaskDetailImportApp", arr);

                }
                if (!string.IsNullOrEmpty(file2))
                {
                    BranchFinancialProductTaskDetailImportApp branchFinancialProductTaskDetailImportApp = new BranchFinancialProductTaskDetailImportApp(db);
                    var data = branchFinancialProductTaskDetailImportApp.Scan(file2, arr);
                    if (data.Count > 0)
                    {
                        return Error(data[0].ErrorReson);
                    }
                    else branchFinancialProductTaskDetailImportApp.ImportEx(file2, arr);
                }
                if (!string.IsNullOrEmpty(file3))
                {
                    BranchCooperateBankTaskDetailImportApp branchCooperateBankTaskDetailImportApp = new BranchCooperateBankTaskDetailImportApp(db);
                    var data = branchCooperateBankTaskDetailImportApp.Scan(file3, arr);
                    if (data.Count > 0)
                    {
                        return Error(data[0].ErrorReson);
                    }
                    else branchCooperateBankTaskDetailImportApp.ImportEx(file3, arr);
                }
                db.Commit();
                
            }
            if (!string.IsNullOrEmpty(file1) && System.IO.File.Exists(file1)) System.IO.File.Delete(file1);
            if (!string.IsNullOrEmpty(file2) && System.IO.File.Exists(file2)) System.IO.File.Delete(file2);
            if (!string.IsNullOrEmpty(file3) && System.IO.File.Exists(file3)) System.IO.File.Delete(file3);


            return Content(new AjaxResult { state = ResultType.success, message = "操作成功。" }.ToJson());

        }
        #endregion

        #region 获取年度任务  (公司、个人)
        /// <summary>
        /// 2.26.获取年度任务
        /// </summary>
        /// <param name="task_name">任务名称</param>
        /// <param name="task_statue">任务分发状态</param>
        /// <param name="user_id">当前操作者id</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetYearlyTask(string task_id, int task_type, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            var data = yearlyTaskDetailApp.GetYearlyTaskDetailOverview(pg, task_id, task_type);
            return Success("操作成功。", data, pg.records);
        }
        #endregion


        #region 获取金融产品任务
        /// <summary>
        /// 2.26.获取年度任务
        /// </summary>
        /// <param name="task_name">任务名称</param>
        /// <param name="task_statue">任务分发状态</param>
        /// <param name="user_id">当前操作者id</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFinanicalProductsTask(string task_id, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            var data = branchFinancialProductTaskDetailApp.GetFinanicalProductsTaskDetailOverview(pg, task_id);
            return Success("操作成功。", data, pg.records);
        }
        #endregion

        #region 获取合作银行任务
        /// <summary>
        /// 2.26.获取年度任务
        /// </summary>
        /// <param name="task_name">任务名称</param>
        /// <param name="task_statue">任务分发状态</param>
        /// <param name="user_id">当前操作者id</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCoopertionBankTask(string task_id, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            var data = branchCoopertionBankTaskDetailApp.GetCoopertionBankTaskDetailOverview(pg, task_id);
            return Success("操作成功。", data, pg.records);
        }
        #endregion


        #region 下载任务模板
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DownTaskTemplete(int task_type, int templete_type)
        {
            if (templete_type == 1)
            {

                return DownLoadYearlyTaskTemplete(task_type);
            }
            if (templete_type == 2)
            {
                return DownFinancialProductTaskTemplete();
            }
            if (templete_type == 3)
            {
                return DownLoadCooperationBankTaskTemplete();
            }
            return Success("操作成功。", null);
        }
        #endregion


        #region 下载全年指标模板
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DownLoadYearlyTaskTemplete(int task_type)
        {
            //模板名称
            string templeteName = "全年任务指标";

            //模板各列名称
            string[] listColumn = new string[] { };
            if (task_type == 0)
            {
                listColumn = Constant.YearlyTaskTempleteCompanyListColumn;
            }
            else {
                listColumn = Constant.YearlyTaskTempletePersonListColumn;
            }

            //指定excel格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(templeteName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(templeteName) + "数据录入模板.xls";
            excelconfig.IsAllSizeColumn = true;

            excelconfig.ColumnEntity = new List<ColumnEntity>();
            
            //行数据
            DataTable rowData = new DataTable();
            

            //各个细项指标
            for (int i = 0; i < listColumn.Length; i++)
            {
                string columnName = "YearlyTask_" + i;
                string excelColumnName = listColumn[i];
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = columnName, ExcelColumn = excelColumnName, Alignment = "center" });
                rowData.Columns.Add(columnName);
            }

            if (task_type == 0)
            {
                //获取所有子公司
                OrganizeEntity org = organizeApp.GetForm(BlocId);
                if (org == null)
                {
                    return Error("配置文件中的总公司id不存在，无法生产下载模板！");
                }
                List<OrganizeEntity> branchList = organizeApp.GetListByParentIdOrderById(org.F_Id,CompanyId);
                DataRow dr = rowData.NewRow();
                for (int i = 0; i < branchList.Count; i++)
                {
                    dr = rowData.NewRow();
                    dr["YearlyTask_0"] = branchList[i].F_FullName;
                    rowData.Rows.Add(dr);
                }
            }
            else
            {
                //获取所有当前公司下客户经理
                List<UserEntity> managerList = null;
                if (OperatorProvider.Provider.GetCurrent() != null)
                {
                    managerList = userApp.GetByOrgIdAndCMRoleId(OperatorProvider.Provider.GetCurrent().CompanyId,CustomerManagerRoleId);
                    if (managerList.Count == 0) return Error("此公司下暂无客户经理，无法生成Excel模板！");
                }
                else
                {
                    return Error("无法获取当前登录人信息！");
                }

                //List<UserEntity> managerList = userApp.GetList();
                
                for (int i = 0; i < managerList.Count; i++)
                {
                    DataRow dr = rowData.NewRow();
                    dr = rowData.NewRow();
                    dr["YearlyTask_0"] = managerList[i].F_RealName;
                    dr["YearlyTask_14"] = managerList[i].F_MobilePhone;
                    rowData.Rows.Add(dr);
                }
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
            return Success("操作成功。", null);
        }
        #endregion



        #region 下载金融产品分类指标模板
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DownFinancialProductTaskTemplete()
        {
            //模板名称
            string templeteName = "金融产品分类指标";
            //模板各列名称


            string[] listColumn = Constant.FinancialProductTaskTempleteListColumn;

            //指定excel格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(templeteName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(templeteName) + "数据录入模板.xls";
            excelconfig.IsAllSizeColumn = true;

            excelconfig.ColumnEntity = new List<ColumnEntity>();
            
            //行数据
            DataTable rowData = new DataTable();
            //rowData.Columns.Add("branch_name");
            //各个细项指标
            for (int i = 0; i < listColumn.Length; i++)
            {
                string columnName = "FinancialProduct_" + i;
                string excelColumnName = listColumn[i];
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = columnName, ExcelColumn = excelColumnName, Alignment = "center" });
                rowData.Columns.Add(columnName);
            }

            //获取所有子公司
            OrganizeEntity org = organizeApp.GetForm(BlocId);
            if (org == null)
            {
                return Error("配置文件中的总公司id不存在，无法生产下载模板！");
            }
            List<OrganizeEntity> branchList = organizeApp.GetListByParentIdOrderById(org.F_Id, CompanyId);
            DataRow dr = rowData.NewRow();
            for (int i = 0; i < branchList.Count; i++)
            {
                dr = rowData.NewRow();
                dr["FinancialProduct_0"] = branchList[i].F_FullName;
                rowData.Rows.Add(dr);
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
            return Success("操作成功。", null);
        }
        #endregion

        #region 下载合作银行模板
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DownLoadCooperationBankTaskTemplete()
        {
            //模板名称
            string templeteName = "合作银行分配指标";
            //模板各列名称
            string[] listColumn = Constant.CooperationBankTaskTempleteListColumn;

            //指定excel格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(templeteName);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(templeteName) + "考核数据录入模板.xls";
            excelconfig.IsAllSizeColumn = true;

            excelconfig.ColumnEntity = new List<ColumnEntity>();
            
            //行数据
            DataTable rowData = new DataTable();
            //rowData.Columns.Add("branch_name");
            //各个细项指标
            for (int i = 0; i < listColumn.Length; i++)
            {
                string columnName = "CooperationBank_" + i;
                string excelColumnName = listColumn[i];
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = columnName, ExcelColumn = excelColumnName, Alignment = "center" });
                rowData.Columns.Add(columnName);
            }

            //获取所有子公司
                OrganizeEntity org = organizeApp.GetForm(BlocId);
            if (org == null)
            {
                return Error("配置文件中的总公司id不存在，无法生产下载模板！");
            }
            List<OrganizeEntity> branchList = organizeApp.GetListByParentIdOrderById(org.F_Id, CompanyId);
            DataRow dr = rowData.NewRow();
            for (int i = 0; i < branchList.Count; i++)
            {
                dr = rowData.NewRow();
                dr["CooperationBank_0"] = branchList[i].F_FullName;
                rowData.Rows.Add(dr);
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
            return Success("操作成功。", null);
        }
        #endregion
    }
}