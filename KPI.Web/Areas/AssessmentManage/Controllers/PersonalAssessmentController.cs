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
    public class PersonalAssessmentController : ControllerBase
    {

        private string PersonalAssessmentManagerRoleId = Configs.GetValue("PersonalAssessmentManagerRoleId");

        private AssessmentApp assessmentApp = new AssessmentApp();
        private AssessmentDetailApp assessmentDetailApp = new AssessmentDetailApp();
        private AssessmentResultApp assessmentResultApp = new AssessmentResultApp();
        private OrganizeApp organizeApp = new OrganizeApp();
        private TempleteApp templeteApp = new TempleteApp();
        private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();
        private DimensionDetailApp dimensionDetailApp = new DimensionDetailApp();
        private DutyApp dutyApp = new DutyApp();
        private SelfTaskDetailApp selfTaskDetailApp = new SelfTaskDetailApp();
        private AssessmentResultCountApp assessmentResultCountApp = new AssessmentResultCountApp();
        private UserApp userApp = new UserApp();

        #region 考核管理——个人 总览视图
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPersonalAssessmentOverview(string assessment_name, string assessment_sponsor, int? assessment_statue, string pagination)
        {
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            pg.sidx = "create_time";
            pg.sord = "desc";
            List<AssessmentOverviewModel> retData = assessmentApp.GetAssessmentOverview(assessment_name, assessment_sponsor, assessment_statue, pg, 0, OperatorProvider.Provider.GetCurrent().CompanyId);
            return Success("操作成功。", retData, pg.records);
        }
        #endregion

        #region 考核管理——个人 详细视图
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPersonalAssessmentDetail(string assessment_id, string personal_name, string telphone, int? assessment_detail_statue, string pagination)
        {
            string checkObj = null;
            //如果当前登陆人不是“个人考核管理员角色的话”只能看自己的记录
            if (OperatorProvider.Provider.GetCurrent().RoleId != PersonalAssessmentManagerRoleId)
                checkObj = OperatorProvider.Provider.GetCurrent().UserId;
            Pagination pg = KPI.Code.Json.ToObject<Pagination>(pagination);
            List<PersonalAssessmentDetailOverviewModel> retData = assessmentDetailApp.GetPersonalAssessmentDetail(assessment_id, personal_name, telphone, assessment_detail_statue, checkObj, pg);
            return Success("操作成功。", retData, pg.records);
        }
        #endregion

        #region 发起个人考核
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult LaunchPersonalAssessment(string assessment_name, string start_time, string end_time, string templete_id, string checker_object_list, int? need_self_check, double? self_check_weight, string checker_list, string filling_people, string file_name)
        {
            if (assessmentApp.IsExists(assessment_name)) return Error("考核名称已存在。");
            using (var db = new RepositoryBase().BeginTrans())
            {
                List<string> checkerObjectList = KPI.Code.Json.ToObject<List<string>>(checker_object_list);
                FileLog.Debug(checker_object_list);
                List<PersonalAssessmentCheckerModel> checkerList = KPI.Code.Json.ToObject<List<PersonalAssessmentCheckerModel>>(checker_list);
                #region 插入考核发起表
                AssessmentEntity assessmentEntity = new AssessmentEntity();
                string assessmentId = Common.GuId();
                assessmentEntity.id = assessmentId;
                assessmentEntity.assessment_name = assessment_name;
                assessmentEntity.templete_id = templete_id;
                assessmentEntity.start_time = DateTime.Parse(start_time);
                assessmentEntity.end_time = DateTime.Parse(end_time);
                assessmentEntity.assessment_sponsor = OperatorProvider.Provider.GetCurrent().UserId;
                assessmentEntity.assessment_count = checkerObjectList.Count;
                assessmentEntity.need_self_check = need_self_check.HasValue ? need_self_check.Value : 0;
                assessmentEntity.assessment_statue = 0;
                assessmentEntity.assessment_type = 0;
                assessmentEntity.filing_people = filling_people;
                assessmentEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                assessmentEntity.create_time = DateTime.Now;
                db.Insert<AssessmentEntity>(assessmentEntity);
                #endregion
                #region 插入考核发起详细表
                AssessmentDetailEntity assessmentDetailEntity = null;
                //如果需要自评
                if (assessmentEntity.need_self_check == 1)
                {
                    for (int i = 0; i < checkerObjectList.Count; i++)
                    {
                        string checkObject = checkerObjectList[i];
                        assessmentDetailEntity = new AssessmentDetailEntity();
                        assessmentDetailEntity.id = Common.GuId();
                        assessmentDetailEntity.assessment_id = assessmentId;
                        assessmentDetailEntity.check_object = checkObject;
                        assessmentDetailEntity.checker = checkObject;
                        assessmentDetailEntity.check_order = 1;
                        assessmentDetailEntity.finished = 0;
                        assessmentDetailEntity.checker_weight = self_check_weight;
                        assessmentDetailEntity.check_total_count = checkerList.Count;
                        assessmentDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        assessmentDetailEntity.create_time = DateTime.Now;
                        db.Insert<AssessmentDetailEntity>(assessmentDetailEntity);

                        //插入“自评”任务并且激活
                        SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                        selfTaskDetailEntity.id = Common.GuId();
                        selfTaskDetailEntity.task_statue = 1;
                        selfTaskDetailEntity.task_type = 3;
                        selfTaskDetailEntity.person = checkObject;
                        selfTaskDetailEntity.task_object = checkObject;
                        selfTaskDetailEntity.attach = assessmentId;
                        selfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        selfTaskDetailEntity.create_time = DateTime.Now;
                        db.Insert<SelfTaskDetailEntity>(selfTaskDetailEntity);

                    }
                }
                for (int i = 0; i < checkerList.Count; i++)
                {
                    for (int j = 0; j < checkerObjectList.Count; j++)
                    {
                        PersonalAssessmentCheckerModel checker = checkerList[i];
                        string checkObject = checkerObjectList[j];
                        assessmentDetailEntity = new AssessmentDetailEntity();
                        assessmentDetailEntity.id = Common.GuId();
                        assessmentDetailEntity.assessment_id = assessmentId;
                        assessmentDetailEntity.check_object = checkObject;
                        assessmentDetailEntity.checker = checker.checker_id;
                        assessmentDetailEntity.check_order = checker.checker_order;
                        assessmentDetailEntity.finished = 0;
                        assessmentDetailEntity.checker_weight = checker.checker_weight;
                        assessmentDetailEntity.check_total_count = checkerList.Count;
                        assessmentDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        assessmentDetailEntity.create_time = DateTime.Now;
                        db.Insert<AssessmentDetailEntity>(assessmentDetailEntity);


                        //给所有“考评人”插入考评任务
                        SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                        selfTaskDetailEntity.id = Common.GuId();
                        if (checker.checker_order == 1)//第一个人直接激活任务
                            selfTaskDetailEntity.task_statue = 1;
                        else //剩下的人暂时先不激活任务，等待上一个人完成任务后激活下一个人的任务
                            selfTaskDetailEntity.task_statue = 0;
                        selfTaskDetailEntity.task_type = 1;
                        selfTaskDetailEntity.person = checker.checker_id;
                        selfTaskDetailEntity.task_object = checkObject;
                        selfTaskDetailEntity.attach = assessmentId;
                        selfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        selfTaskDetailEntity.create_time = DateTime.Now;
                        db.Insert<SelfTaskDetailEntity>(selfTaskDetailEntity);
                    }
                }
                #endregion

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

                PersonalAssessmentImportApp personalAssessmentImportApp = new PersonalAssessmentImportApp(db);
                var data = personalAssessmentImportApp.Scan(file_name, new string[] { assessmentId });
                if (data.Count > 0) return Error("导入数据存在未处理的错误！");
                else personalAssessmentImportApp.Import(file_name, assessmentId);
                db.Commit();
            }
            return Success("操作成功。", null);
        }


        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ReLaunchPersonalAssessment(string assessment_id, string file_name)
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
                    list.ForEach((tt) =>
                    {
                        if (t.person == tt.checker)
                            t.task_statue = 1;
                    });
                });

                //删除上次考核成绩
                assessmentResultCountList.ForEach((t) =>
                {
                    db.Delete<AssessmentResultCountEntity>(t);
                });

                //导入考核数据
                PersonalAssessmentImportApp personalAssessmentImportApp = new PersonalAssessmentImportApp(db);
                var data = personalAssessmentImportApp.Scan(file_name, new string[] { assessment_id });
                if (data.Count > 0) return Error("导入数据存在未处理的错误！");
                else personalAssessmentImportApp.Import(file_name, assessment_id);

                db.Commit();
            }
            return Success("操作成功。", null);
        }



        public class PersonalAssessmentCheckerModel
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
            /// <summary>
            /// 权重
            /// </summary>
            public double checker_weight { get; set; }
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DownLoadPersonalAssessmentTemplete(string check_object_list, string templete_id, string assessment_name)
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
            //个人信息
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "F_RealName", ExcelColumn = "员工姓名", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "F_MobilePhone", ExcelColumn = "手机号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "F_Duty", ExcelColumn = "职位", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "F_DepartmentId", ExcelColumn = "所属组织", Alignment = "center" });
            //行数据
            DataTable rowData = new DataTable();
            rowData.Columns.Add("F_RealName");
            rowData.Columns.Add("F_MobilePhone");
            rowData.Columns.Add("F_Duty");
            rowData.Columns.Add("F_DepartmentId");
            //各个细项指标
            for (int i = 0; i < indicatorsDefineList.Count; i++)
            {
                string indicatorName = indicatorsDefineList[i];
                string columnName = "indicator_" + i;
                string excelColumnName = indicatorName;
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = columnName, ExcelColumn = excelColumnName, Alignment = "center" });
                rowData.Columns.Add(columnName);
            }
            //转化所有考核对象列表
            List<string> checkObjectIdList = KPI.Code.Json.ToObject<List<string>>(check_object_list);
            DataRow dr = rowData.NewRow();
            for (int i = 0; i < checkObjectIdList.Count; i++)
            {
                UserEntity userEntity = userApp.GetForm(checkObjectIdList[i]);
                dr = rowData.NewRow();
                dr["F_RealName"] = userEntity.F_RealName;
                dr["F_MobilePhone"] = userEntity.F_MobilePhone;
                dr["F_Duty"] = dutyApp.GetForm(userEntity.F_DutyId).F_FullName;
                dr["F_DepartmentId"] = organizeApp.GetForm(userEntity.F_DepartmentId).F_FullName;
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
        public ActionResult GetPersonalAssessmentTempResult(string assessment_id, string checker, string check_object)
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
        public ActionResult GetPersonalAssessmentTempleteIsNeedSelfCheck(string templete_id)
        {
            //根据考核模板获取所有的指标
            //根据模板id查找该模板的所有细项指标
            List<DimensionDetailEntity> dimensionDetailList = dimensionDetailApp.GetTempleteDimensionDetail(templete_id);
            List<string> indicatorsDefineList = new List<string>();
            bool result = false;
            for (int i = 0; i < dimensionDetailList.Count; i++)
            {
                DimensionDetailEntity entity = dimensionDetailList[i];
                string formule = entity.formule;
                string checkMethodId = entity.method_id;
                //判断条件公式空 或者考核办法为空
                if (formule.Contains("@"))
                {
                    result = true;
                    break;
                }
            }
            return Success("操作成功。", result);
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPersonalAssessmentTempleteTempResult(string assessment_id, string check_object)
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
                            model.finish = 0d;
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

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult CheckAssessment(string assessment_id, string check_object, string checker_result_list, double? fixed_score, double? unfixed_score, double? total_score, double? checker_weight)
        {
            AssessmentEntity assessmentEntity = assessmentApp.GetForm(assessment_id);
            if (assessmentEntity == null) return Error("此考核不存在。");
            //if (assessmentEntity.need_self_check == 0) return Error("此考核不需要自评。");
            List<SelfCheckResultModel> resultList = KPI.Code.Json.ToObject<List<SelfCheckResultModel>>(checker_result_list);
            string checker = OperatorProvider.Provider.GetCurrent().UserId;

            using (var db = new RepositoryBase().BeginTrans())
            {
                AssessmentDetailEntity assessmentDetailEntity = assessmentDetailApp.GetByAssessmentIdCheckerCheckObj(assessment_id, checker, check_object);
                assessmentDetailEntity.finished = 1;
                assessmentDetailEntity.last_modify_time = DateTime.Now;
                db.Update<AssessmentDetailEntity>(assessmentDetailEntity);

                //更新自己的考核任务为已完成
                SelfTaskDetailEntity curTaskDetailEntity = null;
                if (checker == check_object && assessmentDetailApp.IsSelfCheck(assessment_id, checker, check_object))//自评
                    curTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachPersonTaskObjTaskType(assessment_id, checker, check_object, 3);
                else
                    curTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachPersonTaskObjTaskType(assessment_id, checker, check_object, 1);
                curTaskDetailEntity.task_statue = 2;
                curTaskDetailEntity.last_modify_time = DateTime.Now;
                db.Update<SelfTaskDetailEntity>(curTaskDetailEntity);

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

                //如果自己不是该考核对象的最后一个考核人激活下一个人对该考核对象的任务
                if (!assessmentDetailApp.IsLastCheckerToObject(assessment_id, checker, check_object))
                {

                    int nextCheckOrder = assessmentDetailEntity.check_order + 1;
                    AssessmentDetailEntity nextAssessmentDetailEntity = assessmentDetailApp.GetByAssessmentIdCheckOrderCheckObj(assessment_id, check_object, nextCheckOrder);
                    SelfTaskDetailEntity selfTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachPersonTaskObjTaskType(assessment_id, nextAssessmentDetailEntity.checker, check_object, 1);
                    //激活任务
                    selfTaskDetailEntity.task_statue = 1;
                    selfTaskDetailEntity.last_modify_time = DateTime.Now;
                    db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);

                }
                //如果是整个考核的最后一条
                if (assessmentDetailApp.IsLastChecker(assessment_id))
                {
                    //更新整个考核为待归档
                    AssessmentEntity curAssessmentEntity = assessmentApp.GetForm(assessment_id);
                    if (curAssessmentEntity == null) return Error("系统未知错误。");
                    curAssessmentEntity.assessment_statue = 1;
                    curAssessmentEntity.last_modify_time = DateTime.Now;
                    db.Update<AssessmentEntity>(curAssessmentEntity);
                    //激活“考核结果审核人”的归档任务
                    SelfTaskDetailEntity selfTaskDetailEntity = selfTaskDetailApp.GetByPersonAttachTaskObjTaskType(assessment_id, assessment_id, 4);
                    selfTaskDetailEntity.task_statue = 1;
                    selfTaskDetailEntity.last_modify_time = DateTime.Now;
                    db.Update<SelfTaskDetailEntity>(selfTaskDetailEntity);
                }
                //更新自评项分数
                for (int i = 0; i < resultList.Count; i++)
                {
                    SelfCheckResultModel model = resultList[i];
                    model.indicator_id = indicatorsDefineApp.GetIndicatorByName(model.indicator_name).id;
                    AssessmentResultEntity assessmentResultEntity = assessmentResultApp.GetByAssessmentIdObjIndicatorId(assessment_id, check_object, model.indicator_id);
                    assessmentResultEntity.indicator_value = model.indicator_value;
                    assessmentResultEntity.last_modify_time = DateTime.Now;
                    db.Update<AssessmentResultEntity>(assessmentResultEntity);
                }
                db.Commit();
            }
            return Success("操作成功。");
        }

        public class SelfCheckResultModel
        {
            public string indicator_name { get; set; }
            public string indicator_id { get; set; }
            public double? indicator_value { get; set; }
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPersonalCheckerResultDetail(string assessment_id, string check_object)
        {
            List<ResultDetailMode> list = new List<ResultDetailMode>();

            List<AssessmentDetailEntity> assessmentDetailEntityList = assessmentDetailApp.GetByAssessmentIdCheckObj(assessment_id, check_object);
            assessmentDetailEntityList = assessmentDetailEntityList.OrderBy(t => t.check_order).ToList();
            List<AssessmentDetailEntity> unCheckedList = assessmentDetailEntityList.FindAll(t => t.finished == 0);
            AssessmentDetailEntity curItem = null;
            if (unCheckedList != null && unCheckedList.Count > 0) curItem = unCheckedList[0];
            assessmentDetailEntityList.ForEach((t) =>
            {
                ResultDetailMode resultDetailMode = new ResultDetailMode();
                resultDetailMode.is_checked = t.finished;
                resultDetailMode.check_order = t.check_order;
                resultDetailMode.checker = t.checker;
                resultDetailMode.checker_name = userApp.GetForm(t.checker).F_RealName;
                resultDetailMode.checker_weight = t.checker_weight;

                AssessmentResultCountEntity assessmentResultCountEntity = assessmentResultCountApp.GetByAssessmentIdCheckerCheckObject(assessment_id, t.checker, t.check_object);
                if (assessmentResultCountEntity != null)
                    resultDetailMode.result = assessmentResultCountEntity.unfixed_score;
                else resultDetailMode.result = 0d;
                if (curItem != null)
                    if (t.id == curItem.id) resultDetailMode.is_checked_true = 1; else resultDetailMode.is_checked_true = 0;
                else
                    resultDetailMode.is_checked_true = 0;
                list.Add(resultDetailMode);
            });
            return Success("操作成功。", list, list.Count);
        }

        public class ResultDetailMode
        {
            public int? is_checked { get; set; }
            public int? check_order { get; set; }
            public string checker { get; set; }
            public string checker_name { get; set; }
            public double? checker_weight { get; set; }
            public double? result { get; set; }
            public int? is_checked_true { get; set; }

        }


        #endregion

        #region 首页折线图
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPersonalYearlyAssessmentResult(string person, string year)
        {
            string personName = OperatorProvider.Provider.GetCurrent().UserName;
            string branch = organizeApp.GetForm(OperatorProvider.Provider.GetCurrent().CompanyId).F_FullName;
            string titleStr = year + "年" + branch + "客户经理" + personName + "考核情况";
            List<MonthAssessmentResultMode> list = new List<MonthAssessmentResultMode>();
            for (int i = 1; i < 13; i++)
            {
                List<AssessmentResultCountEntity> assessmentResultCountList = assessmentResultCountApp.GetMonthAssessment(OperatorProvider.Provider.GetCurrent().UserId, i + "");
                if (assessmentResultCountList != null && assessmentResultCountList.Count > 0)
                {
                    MonthAssessmentResultMode model = new MonthAssessmentResultMode();
                    model.title = titleStr;
                    model.month = i;
                    double? totalResult = 0d;
                    totalResult += assessmentResultCountList.First().fixed_score;
                    for (int j = 0; j < assessmentResultCountList.Count; j++)
                    {
                        totalResult += assessmentResultCountList[j].unfixed_score * assessmentResultCountList[j].checker_weight / 100d;
                    }
                    model.result = (double?)decimal.Round(decimal.Parse(totalResult.ToString()), 2);
                    list.Add(model);
                }
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
        public ActionResult SavePersonalAssessmentResult(string assessment_id)
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
    }
}