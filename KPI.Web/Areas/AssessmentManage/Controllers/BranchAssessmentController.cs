using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KPI.Domain.Entity.AssessmentManage;
using KPI.Domain.Entity.SystemManage;
using KPI.Domain.Entity.TempleteManage;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.Entity.TaskManage;
using KPI.Application.AssessmentManage;
using KPI.Application.SystemManage;
using KPI.Application.IndicatorManage;
using KPI.Application.TempleteManage;
using KPI.Application.TaskManage;
using System.Web.Mvc;
using KPI.Code;
using System.Globalization;
using KPI.Domain.ViewModel;
using System.Dynamic;
using System.Text;
using KPI.Data;
using System.Data;
using LuaInterface;
using LuaMysql;

namespace KPI.Web.Areas.AssessmentManage.Controllers
{
    public class BranchAssessmentController : ControllerBase
    {
        //集团再机构表中的主键
        private readonly string BlocId = Configs.GetValue("BlocId");
        private readonly string CompanyId = Configs.GetValue("CompanyId");

        private AssessmentApp assessmentApp = new AssessmentApp();
        private AssessmentDetailApp assessmentDetailApp = new AssessmentDetailApp();
        private AssessmentResultApp assessmentResultApp = new AssessmentResultApp();
        private OrganizeApp organizeApp = new OrganizeApp();
        private TempleteApp templeteApp = new TempleteApp();
        private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();
        private DimensionDetailApp dimensionDetailApp = new DimensionDetailApp();
        private UserApp userApp = new UserApp();
        private SelfTaskDetailApp selfTaskDetailApp = new SelfTaskDetailApp();
        private AssessmentResultCountApp assessmentResultCountApp = new AssessmentResultCountApp();

        //
        #region 考核管理——分公司 总览视图
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchAssessmentOverview(string assessment_name, string assessment_sponsor, int? assessment_statue, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            List<AssessmentOverviewModel> retData = assessmentApp.GetAssessmentOverview(assessment_name, assessment_sponsor, assessment_statue, pg, 1);
            return Success("操作成功。", retData, pg.records);
        }
        #endregion

        #region 考核管理——分公司 详细视图
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchAssessmentDetail(string assessment_id, string branch_name, int? assessment_detail_statue, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            List<BranchAssessmentDetailOverviewModel> retData = assessmentDetailApp.GetBranchAssessmentDetail(assessment_id, branch_name, assessment_detail_statue, pg);
            return Success("操作成功。", retData, pg.records);
        }
        #endregion

        #region 发起分公司考核
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult LaunchBranchAssessment(string assessment_name, string start_time, string end_time, string templete_id, string checker_list, string filling_people, string file_name)
        {
            if (assessmentApp.IsExists(assessment_name)) return Error("考核名称已存在。");
            using (var db = new RepositoryBase().BeginTrans())
            {
                //获取“中安金控集团id”
                OrganizeEntity org = organizeApp.GetForm(BlocId);
                //获取所有子公司
                List<OrganizeEntity> branchList = organizeApp.GetListByParentIdOrderById(org.F_Id, CompanyId);
                List<BranchAssessmentCheckerModel> checkerList = KPI.Code.Json.ToObject<List<BranchAssessmentCheckerModel>>(checker_list);
                #region 插入考核发起表
                AssessmentEntity assessmentEntity = new AssessmentEntity();
                string assessmentId = Common.GuId();
                assessmentEntity.id = assessmentId;
                assessmentEntity.assessment_name = assessment_name;
                assessmentEntity.templete_id = templete_id;
                assessmentEntity.start_time = DateTime.Parse(start_time);
                assessmentEntity.end_time = DateTime.Parse(end_time);
                assessmentEntity.assessment_sponsor = OperatorProvider.Provider.GetCurrent().UserId;
                assessmentEntity.assessment_count = branchList.Count;
                assessmentEntity.assessment_statue = 0;
                assessmentEntity.assessment_type = 1;
                assessmentEntity.filing_people = filling_people;
                assessmentEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                assessmentEntity.create_time = DateTime.Now;
                db.Insert<AssessmentEntity>(assessmentEntity);
                #endregion
                #region 插入考核发起详细表
                AssessmentDetailEntity assessmentDetailEntity = new AssessmentDetailEntity();
                for (int i = 0; i < checkerList.Count; i++)
                {
                    for (int j = 0; j < branchList.Count; j++)
                    {
                        BranchAssessmentCheckerModel checker = checkerList[i];
                        OrganizeEntity check_object = branchList[j];
                        assessmentDetailEntity = new AssessmentDetailEntity();
                        assessmentDetailEntity.id = Common.GuId();
                        assessmentDetailEntity.assessment_id = assessmentId;
                        assessmentDetailEntity.check_object = check_object.F_Id;
                        assessmentDetailEntity.checker = checker.checker_id;
                        assessmentDetailEntity.check_order = checker.checker_order;
                        assessmentDetailEntity.finished = 0;
                        assessmentDetailEntity.checker_weight = 0;
                        assessmentDetailEntity.check_total_count = checkerList.Count;
                        assessmentDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        assessmentDetailEntity.create_time = DateTime.Now;
                        db.Insert<AssessmentDetailEntity>(assessmentDetailEntity);

                        //给所有“考评人”插入考评任务并且激活第一个考评人的任务
                        SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                        selfTaskDetailEntity.id = Common.GuId();
                        if (checker.checker_order == 1)//第一个人直接激活任务
                            selfTaskDetailEntity.task_statue = 1;
                        else //剩下的人暂时先不激活任务，等待上一个人完成任务后激活下一个人的任务
                            selfTaskDetailEntity.task_statue = 0;
                        selfTaskDetailEntity.task_type = 1;
                        selfTaskDetailEntity.person = checker.checker_id;
                        selfTaskDetailEntity.task_object = check_object.F_Id;
                        selfTaskDetailEntity.attach = assessmentId;
                        selfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        selfTaskDetailEntity.create_time = DateTime.Now;
                        db.Insert<SelfTaskDetailEntity>(selfTaskDetailEntity);

                    }
                }

                //给“考核结果审核人插入归档任务”

                SelfTaskDetailEntity fillingSelfTaskDetailEntity = new SelfTaskDetailEntity();
                fillingSelfTaskDetailEntity.id = Common.GuId();
                fillingSelfTaskDetailEntity.task_statue = 0;
                fillingSelfTaskDetailEntity.task_type = 4;
                fillingSelfTaskDetailEntity.person = filling_people;
                fillingSelfTaskDetailEntity.task_object = assessmentId;
                fillingSelfTaskDetailEntity.attach = assessmentId;
                fillingSelfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                fillingSelfTaskDetailEntity.create_time = DateTime.Now;
                db.Insert<SelfTaskDetailEntity>(fillingSelfTaskDetailEntity);

                //导入考核数据
                BranchAssessmentImportApp branchAssessmentImportApp = new BranchAssessmentImportApp(db);
                var data = branchAssessmentImportApp.Scan(file_name, new string[] { assessmentId });
                if (data.Count > 0) return Error("导入数据存在未处理的错误！");
                else branchAssessmentImportApp.Import(file_name, assessmentId);
                db.Commit();
            }
            #endregion
            return Success("操作成功。", null);
        }


        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ReLaunchBranchAssessment(string assessment_id, string file_name)
        {
            AssessmentEntity assessmentEntity = assessmentApp.GetForm(assessment_id);
            if (assessmentEntity == null) return Error("系统未知错误。");
            //if (assessmentEntity.assessment_statue != 1) return Error("只有处于待归档的考核才能重新发起。");
            List<AssessmentDetailEntity> assessmentDetailList = assessmentDetailApp.GetListByAssessmentId(assessment_id);
            if (assessmentDetailList == null || assessmentDetailList.Count <= 0) return Error("系统未知错误。");
            List<SelfTaskDetailEntity> selfTaskDetailList = selfTaskDetailApp.GetListByAttach(assessment_id);
            if (selfTaskDetailList == null || selfTaskDetailList.Count <= 0) return Error("系统未知错误。");
            List<AssessmentResultCountEntity> assessmentResultCountList = assessmentResultCountApp.GetByAssessmentId(assessment_id);
            if (assessmentResultCountList == null || assessmentResultCountList.Count <= 0) return Error("系统未知错误。");
            using (var db = new RepositoryBase().BeginTrans())
            {
                //将本次考核状态设置为初始状态 0 
                assessmentEntity.assessment_statue = 0;
                assessmentEntity.last_modify_time = DateTime.Now;
                db.Update<AssessmentEntity>(assessmentEntity);
                //更新考核发起详细表
                assessmentDetailList.ForEach((t) =>
                {
                    t.finished = 0;
                    t.last_modify_time = DateTime.Now;
                    db.Update<AssessmentDetailEntity>(t);
                });
                //1.更新所有任务表状态为未激活
                selfTaskDetailList.ForEach((t) =>
                {
                    t.task_statue = 0;
                    t.last_modify_time = DateTime.Now;
                    db.Update<SelfTaskDetailEntity>(t);
                });
                //2.更新check_order为1的任务状态为已激活
                selfTaskDetailList.ForEach((t) =>
                {
                    List<AssessmentDetailEntity> list = assessmentDetailApp.GetFirstCheckerList(assessment_id);
                    list.ForEach((tt) => {
                        if(t.person == tt.checker)
                            t.task_statue = 1;
                    });
                });

                //删除上次考核成绩
                assessmentResultCountList.ForEach((t) =>
                {
                    db.Delete<AssessmentResultCountEntity>(t);
                });

                //导入考核数据
                BranchAssessmentImportApp branchAssessmentImportApp = new BranchAssessmentImportApp(db);
                var data = branchAssessmentImportApp.Scan(file_name, new string[] { assessment_id });
                if (data.Count > 0) return Error("导入数据存在未处理的错误！");
                else branchAssessmentImportApp.Import(file_name, assessment_id);

                db.Commit();
            }
            return Success("操作成功。", null);
        }


        public class BranchAssessmentCheckerModel
        {
            /// <summary>
            /// 考评人id
            /// </summary>
            public string checker_id { get; set; }
            /// <summary>
            /// 考评人姓名
            /// </summary>
            public string checker_name { get; set; }
            /// <summary>
            /// 考评人次序
            /// </summary>
            public int checker_order { get; set; }
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DownLoadBranchAssessmentTemplete(string templete_id, string assessment_name)
        {
            //根据模板id查找该模板的所有细项指标
            List<DimensionDetailEntity> dimensionDetailList = dimensionDetailApp.GetTempleteDimensionDetail(templete_id);
            List<string> indicatorsDefineList = new List<string>();
            for (int i = 0; i < dimensionDetailList.Count; i++)
            {
                DimensionDetailEntity entity = dimensionDetailList[i];
                string formule = entity.formule;
                List<string> collection = AnalysisFormule(ref formule, null);
                collection.ForEach(item =>
                {
                    indicatorsDefineList.Add(item);
                });
                if (formule.Contains("[") && formule.Contains("]"))//如果有考核办法需要外部附加数据的
                {
                    formule = formule.Replace('[', '|').Replace(']', '|');
                }
                string[] arr = formule.Replace('(', '|').Replace(')', '|').Replace('+', '|').Replace('-', '|').Replace('*', '|').Replace('/', '|').Replace('Σ', '|').Split('|');
                for (int j = 0; j < arr.Length; j++)
                {
                    if (!string.IsNullOrEmpty(arr[j]) && !indicatorsDefineList.Contains(arr[j]))
                        indicatorsDefineList.Add(arr[j]);
                }
            }

            //指定excel格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = Server.UrlDecode(assessment_name);
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.FileName = Server.UrlDecode(assessment_name) + "考核数据录入模板.xls";
            excelconfig.IsAllSizeColumn = true;

            excelconfig.ColumnEntity = new List<ColumnEntity>();
            //写入Excel表头
            //分公司
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "branch_name", ExcelColumn = "分公司名称", Alignment = "center" });
            //行数据
            DataTable rowData = new DataTable();
            rowData.Columns.Add("branch_name");
            //各个细项指标
            for (int i = 0; i < indicatorsDefineList.Count; i++)
            {
                string indicatorName = indicatorsDefineList[i];
                string columnName = "indicator_" + i;
                string excelColumnName = indicatorName;
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = columnName, ExcelColumn = excelColumnName, Alignment = "center" });
                rowData.Columns.Add(columnName);
            }
            //获取所有子公司
            OrganizeEntity org = organizeApp.GetForm(BlocId);
            List<OrganizeEntity> branchList = organizeApp.GetListByParentIdOrderById(org.F_Id, CompanyId);
            DataRow dr = rowData.NewRow();
            for (int i = 0; i < branchList.Count; i++)
            {
                dr = rowData.NewRow();
                dr["branch_name"] = branchList[i].F_FullName;
                rowData.Rows.Add(dr);
            }
            ExcelHelper.ExcelDownload(rowData, excelconfig);
            return Success("操作成功。", null);
        }

        private List<string> AnalysisFormule(ref string formule, List<string> collection)
        {
            if (collection == null) collection = new List<string>();
            if (formule.Contains("^") && formule.Contains("$"))//如果有需要整体公式作为一个指标的
            {
                string segment1 = formule.Substring(0, formule.IndexOf("^"));
                string segment2 = formule.Substring(formule.IndexOf("^"), formule.IndexOf("$") + 1 - formule.IndexOf("^"));
                string segment3 = formule.Substring(formule.IndexOf("$") + 1);
                collection.Add(segment2);
                formule = segment1 + "|" + segment3;
                return AnalysisFormule(ref formule, collection);
            }
            else
                return collection;
        }

        #endregion

        #region 考核

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchAssessmentTempResult(string assessment_id, string checker, string check_object)
        {
            //获取本次考核的考核模板
            string templeteId = assessmentApp.GetByAssessmentId(assessment_id).templete_id;
            //根据考核模板获取所有的指标
            //根据模板id查找该模板的所有细项指标
            List<DimensionDetailEntity> dimensionDetailList = dimensionDetailApp.GetTempleteDimensionDetail(templeteId);
            List<string> indicatorsDefineList = new List<string>();
            for (int i = 0; i < dimensionDetailList.Count; i++)
            {
                DimensionDetailEntity entity = dimensionDetailList[i];
                string formule = entity.formule;
                string[] arr = formule.Replace('(', '|').Replace(')', '|').Replace('+', '|').Replace('-', '|').Replace('*', '|').Replace('/', '|').Replace('Σ', '|').Split('|');
                for (int j = 0; j < arr.Length; j++)
                {
                    if (!string.IsNullOrEmpty(arr[j]) && !indicatorsDefineList.Contains(arr[j]))
                        indicatorsDefineList.Add(arr[j]);
                }
            }
            //根据考核id，考核对象以及指标名称获取当前指标的分数
            //获取最后一个考核人的id
            AssessmentDetailEntity assessmentDetailEntity = assessmentDetailApp.GetLastChecker(assessment_id, check_object);
            string lastChecker = string.Empty;
            if (assessmentDetailEntity != null) lastChecker = assessmentDetailEntity.checker;
            List<AssessmentIndicatorsValueModel> list = assessmentResultApp.GetAllIndicatorsResult(assessment_id, checker, check_object);
            return Success("操作成功。", list, list.Count);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchAssessmentTempleteTempResult(string assessment_id, string check_object)
        {
            List<AssessmentTempleteResultModel> list = assessmentResultApp.GetAssessmentTempleteResultOverview(assessment_id, check_object);
            double? total = 0d;
            //循环根据指标公式和考核办法计算分数
            for (int i = 0; i < list.Count; i++)
            {
                AssessmentTempleteResultModel model = list[i];
                if (model.check_method_proc == ".lua")//使用lua脚本处理
                {
                    using (Lua luavm = new Lua())
                    {
                        luavm["AssessmentId"] = assessment_id;
                        luavm["CheckObject"] = check_object;
                        luavm["BaseScore"] = model.base_score;
                        LuaMysqlHelper db = new LuaMysqlHelper();
                        luavm.RegisterFunction("ExcuteQueryToLuaTable", db, db.GetType().GetMethod("ExcuteQueryToLuaTable"));
                        luavm.RegisterFunction("LuaLog", this, this.GetType().GetMethod("LuaLog"));
                        try
                        {
                            object[] result = luavm.DoFile(model.check_method + ".lua");
                            model.base_standard_score = (double?)decimal.Round(decimal.Parse(result[0].ToString()), 2);
                            model.finish = (double?)decimal.Round(decimal.Parse(result[1].ToString()), 2);
                            model.assessment_result = (double?)decimal.Round(decimal.Parse(result[2].ToString()), 2);
                        }
                        catch (Exception e)
                        {
                            FileLog.Error("计算分数发生异常：" + e);
                            model.base_standard_score = 0d;
                            model.assessment_result = 0d;
                            model.finish = (double?)0d;
                        }
                        total += model.assessment_result;
                        luavm.Dispose();
                    }
                }
                else if (model.check_method_proc == ".procedure")//使用存储过程处理
                {

                }
            }
            //计算总分
            for (int i = 0; i < list.Count; i++)
            {
                AssessmentTempleteResultModel model = list[i];
                model.total_result = (double?)decimal.Round(decimal.Parse(total.ToString()), 2);
            }
            return Success("操作成功。", list, list.Count);
        }

        public void LuaLog(string msg)
        {
            FileLog.Info(msg);
        }


        public string GetLuaTableStringBySql(string connStr, string cmdText)
        {
            LuaMysqlHelper db = new LuaMysqlHelper(connStr);
            return db.ExcuteQueryToLuaTable(cmdText);
        }



        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult CheckAssessment(string assessment_id, string check_object, double? fixed_score, double? unfixed_score, double? toatal_score, double? checker_weight)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string checker = OperatorProvider.Provider.GetCurrent().UserId;
                //
                AssessmentDetailEntity assessmentDetailEntity = assessmentDetailApp.GetByAssessmentIdCheckerCheckObj(assessment_id, checker, check_object);
                assessmentDetailEntity.finished = 1;
                assessmentDetailEntity.last_modify_time = DateTime.Now;
                db.Update<AssessmentDetailEntity>(assessmentDetailEntity);

                //保存此考核人对此考核对象的分数
                AssessmentResultCountEntity assessmentResultCountEntity = new AssessmentResultCountEntity();
                assessmentResultCountEntity.id = Common.GuId();
                assessmentResultCountEntity.assessment_id = assessment_id;
                assessmentResultCountEntity.checker = checker;
                assessmentResultCountEntity.checker_weight = checker_weight;
                assessmentResultCountEntity.check_object = check_object;
                assessmentResultCountEntity.fixed_score = fixed_score;
                assessmentResultCountEntity.unfixed_score = unfixed_score;
                assessmentResultCountEntity.total_score = fixed_score + unfixed_score;
                assessmentResultCountEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                assessmentResultCountEntity.create_time = DateTime.Now;
                db.Insert<AssessmentResultCountEntity>(assessmentResultCountEntity);

                //更新自己的考核任务为已完成
                SelfTaskDetailEntity curTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachPersonTaskObjTaskType(assessment_id, checker, check_object, 1);
                curTaskDetailEntity.task_statue = 2;
                curTaskDetailEntity.last_modify_time = DateTime.Now;
                db.Update<SelfTaskDetailEntity>(curTaskDetailEntity);

                //如果自己不是该考核对象的最后一个考核人激活下一个人对该考核对象的任务
                if (!assessmentDetailApp.IsLastCheckerToObject(assessment_id, checker, check_object))
                {
                    int nextCheckOrder = assessmentDetailEntity.check_order + 1;
                    AssessmentDetailEntity nextAssessmentDetailEntity = assessmentDetailApp.GetByAssessmentIdCheckOrderCheckObj(assessment_id, check_object, nextCheckOrder);
                    SelfTaskDetailEntity curSelfTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachPersonTaskObjTaskType(assessment_id, nextAssessmentDetailEntity.checker, check_object, 1);
                    //激活任务
                    curSelfTaskDetailEntity.task_statue = 1;
                    curSelfTaskDetailEntity.last_modify_time = DateTime.Now;
                    db.Update<SelfTaskDetailEntity>(curSelfTaskDetailEntity);
                }
                //如果是整个考核的最后一条
                if (assessmentDetailApp.IsLastChecker(assessment_id))//如果是最后一个人对最后一个对象考核
                {
                    //更新整个考核为待归档
                    AssessmentEntity assessmentEntity = assessmentApp.GetForm(assessment_id);
                    if (assessmentEntity == null) return Error("系统未知错误。");
                    assessmentEntity.assessment_statue = 1;
                    assessmentEntity.last_modify_time = DateTime.Now;
                    db.Update<AssessmentEntity>(assessmentEntity);
                    //激活“考核结果审核人”的归档任务
                    SelfTaskDetailEntity selfTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachTaskObjTaskType(assessment_id, assessment_id, 4);
                    selfTaskDetailEntity.task_statue = 1;
                    selfTaskDetailEntity.last_modify_time = DateTime.Now;
                    db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                }

                db.Commit();
            }
            return Success("操作成功。", null);
        }
        #endregion

        #region 首页折线图
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchYearlyAssessmentResult(string branch, string year)
        {
            string branchName = organizeApp.GetForm(OperatorProvider.Provider.GetCurrent().CompanyId).F_FullName;
            string titleStr = year + "年" + branchName + "考核情况";
            List<MonthAssessmentResultMode> list = new List<MonthAssessmentResultMode>();
            for (int i = 1; i < 13; i++)
            {
                List<AssessmentResultCountEntity> assessmentResultCountList = assessmentResultCountApp.GetMonthAssessment(OperatorProvider.Provider.GetCurrent().CompanyId, i + "");
                if (assessmentResultCountList != null && assessmentResultCountList.Count > 0)
                    list.Add(new MonthAssessmentResultMode() { title = titleStr, month = i, result = (double?)decimal.Round(decimal.Parse(assessmentResultCountList.First().total_score.ToString()), 2) });
                else
                    list.Add(new MonthAssessmentResultMode() { title = titleStr, month = i, result = 0d });
            }
            return Success("操作成功。", list, list.Count);
        }

        public class MonthAssessmentResultMode
        {
            public string title { get; set; }
            public int month { get; set; }
            public double? result { get; set; }
        }

        #endregion

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SaveBranchAssessmentResult(string assessment_id)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string checker = OperatorProvider.Provider.GetCurrent().UserId;
                AssessmentEntity assessmentEntity = assessmentApp.GetForm(assessment_id);
                if (assessmentEntity == null) return Error("系统未知错误。");
                if (assessmentEntity.assessment_statue != 1) return Error("只有处于待归档的考核才能进行归档。");
                //将本次考核状态设置为已归档 2 
                assessmentEntity.assessment_statue = 2;
                assessmentEntity.last_modify_time = DateTime.Now;
                db.Update<AssessmentEntity>(assessmentEntity);

                //将自己“归档”任务状态设置为已完成
                SelfTaskDetailEntity selfTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachTaskObjTaskType(assessment_id, assessment_id, 4);
                selfTaskDetailEntity.task_statue = 2;
                selfTaskDetailEntity.last_modify_time = DateTime.Now;
                db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                db.Commit();
            }
            return Success("操作成功。", null);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchDefaultChecker()
        {
            List<UserEntity> list = new List<UserEntity>();
            list.Add(userApp.GetForm(Configs.GetValue("DefaultChecker1")));
            list.Add(userApp.GetForm(Configs.GetValue("DefaultChecker2")));
            list.Add(userApp.GetForm(Configs.GetValue("DefaultChecker3")));
            list.Add(userApp.GetForm(Configs.GetValue("DefaultChecker4")));
            return Success("操作成功。", list, list.Count);
        }

        #region 测试linq
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult TestLinq()
        {
            List<Customer> customerList = new List<Customer>();
            customerList.Add(new Customer { username = "张三", role = "1" });
            customerList.Add(new Customer { username = "李四", role = "2" });
            customerList.Add(new Customer { username = "王二", role = "3" });

            List<UserRole> roleList = new List<UserRole>();
            roleList.Add(new UserRole { id = "1", rolename = "超级" });
            roleList.Add(new UserRole { id = "2", rolename = "普通" });
            roleList.Add(new UserRole { id = "3", rolename = "辣鸡" });

            IEnumerable<CustomerRoleView> data;
            data = from customer in customerList
                   join role in roleList on customer.role equals role.id
                   where customer.username == "张三"
                   select new CustomerRoleView
                   {
                       username = customer.username,
                       rolename = role.rolename
                   };
            return Success("操作成功。", data);
        }

        public class Customer
        {
            public string username { get; set; }
            public string role { get; set; }
        }

        public class UserRole
        {
            public string id { get; set; }
            public string rolename { get; set; }
        }

        public class CustomerRoleView
        {
            public string username { get; set; }
            public string rolename { get; set; }
        }
        #endregion
    }
}