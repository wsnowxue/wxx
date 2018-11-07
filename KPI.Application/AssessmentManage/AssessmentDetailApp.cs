using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.IRepository;
using KPI.Repository.AssessmentManage;
using KPI.Domain.Entity.AssessmentManage;
using KPI.Code;
using KPI.Domain.ViewModel;
using System.Dynamic;
using System.Data;

namespace KPI.Application.AssessmentManage
{
    public class AssessmentDetailApp
    {
        private IAssessmentDetailRepository service = new AssessmentDetailRepository();
        private AssessmentResultCountApp assessmentResultCountApp = new AssessmentResultCountApp();

        public List<AssessmentDetailEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.create_time).ToList();
        }
        public AssessmentDetailEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.id == keyValue);
        }
        public void SubmitForm(AssessmentDetailEntity assessmentDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                assessmentDetailEntity.Modify(keyValue);
                service.Update(assessmentDetailEntity);
            }
            else
            {
                assessmentDetailEntity.Create();
                service.Insert(assessmentDetailEntity);
            }
        }

        public List<BranchAssessmentDetailOverviewModel> GetBranchAssessmentDetail(string assessment_id, string branch_name, int? assessment_detail_statue, Pagination pagination)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT DISTINCT a.assessment_id,a.check_object,b.assessment_name,c.templete_name,b.start_time,b.end_time,d.F_FullName AS branch_name,d.F_Id AS branch_id,a.create_time ");
            sqlBuilder.Append("FROM t_kpi_launch_detail a LEFT JOIN t_kpi_launch b ON a.assessment_id = b.id ");
            sqlBuilder.Append("LEFT JOIN t_kpi_templete c ON b.templete_id = c.id ");
            sqlBuilder.Append("LEFT JOIN sys_organize d ON a.check_object = d.F_Id ");
            sqlBuilder.AppendFormat("where a.assessment_id = '{0}' ", assessment_id);
            if (!string.IsNullOrEmpty(branch_name))
                sqlBuilder.AppendFormat("and d.F_FullName like '%{0}%'", branch_name);

            List<BranchAssessmentDetailOverviewModel> list = service.FindListBySql<BranchAssessmentDetailOverviewModel>(sqlBuilder.ToString(), pagination).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                BranchAssessmentDetailOverviewModel model = list[i];
                string checker = OperatorProvider.Provider.GetCurrent().UserId;

                string sql = string.Format("select count(*) from t_self_task_detail where attach = '{0}' and person = '{1}' and task_statue = 1 and task_object = '{2}' ", assessment_id, checker, model.branch_id);
                int taskCount = service.BasicQueryT<int>(sql);
                model.flag = taskCount;

                sql = string.Format("select b.F_RealName from t_kpi_launch_detail a left join sys_user b on a.checker = b.F_Id where a.assessment_id = '{0}' and a.check_object = '{1}' and a.finished = 0 order by a.check_order asc limit 0,1", assessment_id, model.check_object);
                string result = service.BasicQueryT<string>(sql);
                if (!string.IsNullOrEmpty(result))
                {
                    model.assessment_detail_statue = 0;
                    model.assessment_statue_detail = result + "考核中";
                    model.score = 0d;
                }
                else
                {
                    model.assessment_detail_statue = 1;
                    model.assessment_statue_detail = "已完成";
                    //查询成绩（分公司目前成绩多个考评人都一样取第一条）
                    AssessmentResultCountEntity assessmentResultCountEntity = assessmentResultCountApp.GetByAssessmentIdCheckerCheckObject(assessment_id, string.Empty, model.branch_id);
                    //sql = string.Format("select distinct total_score from t_kpi_result where assessment_id = '{0}' and check_object = '{1}'", assessment_id, model.branch_id);
                    //double? totalScore = service.BasicQueryT<double?>(sql);
                    //model.score = assessmentResultCountEntity.total_score;
                    model.score = (double?)decimal.Round(decimal.Parse(assessmentResultCountEntity.total_score.ToString()), 2);
                }

            }

            if (assessment_detail_statue.HasValue)
                list = list.FindAll(t => t.assessment_detail_statue == assessment_detail_statue.Value);

            return list;
        }

        public List<PersonalAssessmentDetailOverviewModel> GetPersonalAssessmentDetail(string assessment_id, string personal_name, string telphone, int? assessment_detail_statue, string check_object, Pagination pagination)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select distinct a.assessment_id,a.check_object,b.assessment_name,c.templete_name,b.start_time,b.end_time,d.F_Id as personal_id,d.F_RealName as personal_name,d.F_MobilePhone as telphone,e.F_FullName as department,d.F_Id as person_id ");
            //sqlBuilder.Append("CASE f.task_statue WHEN 1 THEN 1 ELSE 0 END AS flag ");
            sqlBuilder.Append("from t_kpi_launch_detail a left join t_kpi_launch b on a.assessment_id = b.id ");
            sqlBuilder.Append("left join t_kpi_templete c on b.templete_id = c.id ");
            sqlBuilder.Append("left join sys_user d on a.check_object = d.F_Id ");
            sqlBuilder.Append("left join sys_organize e on d.F_OrganizeId = e.F_Id ");
            //sqlBuilder.Append("LEFT JOIN t_self_task_detail f ON a.assessment_id = f.attach ");
            sqlBuilder.AppendFormat("where a.assessment_id = '{0}' ", assessment_id);
            if (!string.IsNullOrEmpty(personal_name))
                sqlBuilder.AppendFormat(" and d.F_RealName like '%{0}%'", personal_name);
            if (!string.IsNullOrEmpty(telphone))
                sqlBuilder.AppendFormat(" and d.F_MobilePhone like '%{0}%'", telphone);
            if (!string.IsNullOrEmpty(check_object))
                sqlBuilder.AppendFormat(" and d.F_Id = '{0}'", check_object);
            List<PersonalAssessmentDetailOverviewModel> list = service.FindListBySql<PersonalAssessmentDetailOverviewModel>(sqlBuilder.ToString(), pagination).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                PersonalAssessmentDetailOverviewModel model = list[i];

                string checker = OperatorProvider.Provider.GetCurrent().UserId;
                string sql = string.Format("select count(*) from t_self_task_detail where attach = '{0}' and person = '{1}' and task_statue = 1 and task_object = '{2}'", assessment_id, checker, model.check_object);
                int taskCount = service.BasicQueryT<int>(sql);
                model.flag = taskCount;


                sql = string.Format("select a.check_order as check_order,b.F_RealName as checker_name,c.need_self_check as need_self_check from t_kpi_launch_detail a left join sys_user b on a.checker = b.F_Id left join t_kpi_launch c on a.assessment_id = c.id where a.assessment_id = '{0}' and a.check_object = '{1}' and a.finished = 0 order by a.check_order asc limit 0,1", assessment_id, model.check_object);
                PersonalAssessmentDetailModel result = service.BasicQueryT<PersonalAssessmentDetailModel>(sql);
                if (result != null)
                {
                    model.assessment_detail_statue = 0;
                    if (result.need_self_check == 1 && result.check_order == 1)
                        model.assessment_statue_detail = result.checker_name + "自评中";
                    else
                        model.assessment_statue_detail = result.checker_name + "考核中";
                    model.score = 0d;
                }
                else
                {
                    model.assessment_detail_statue = 1;
                    model.assessment_statue_detail = "已完成";

                    //查询成绩
                    double? totalScore = 0d;
                    List<AssessmentResultCountEntity> assessmentResultCountList = assessmentResultCountApp.GetByAssessmentIdCheckObject(assessment_id, model.check_object);
                    assessmentResultCountList.ForEach((t) =>
                    {
                        AssessmentDetailEntity assessmentDetailEntity = GetByAssessmentIdCheckerCheckObj(t.assessment_id, t.checker, t.check_object);
                        totalScore += assessmentDetailEntity.checker_weight / 100d * (t.unfixed_score + t.fixed_score);
                    });
                    //model.score = totalScore;
                    model.score = (double?)decimal.Round(decimal.Parse(totalScore.ToString()), 2);
                }

            }
            if (assessment_detail_statue.HasValue)
                list = list.FindAll(t => t.assessment_detail_statue == assessment_detail_statue.Value);
            return list;
        }

        public class PersonalAssessmentDetailModel
        {
            public int? check_order { get; set; }
            public string checker_name { get; set; }
            public int? need_self_check { get; set; }
        }



        public AssessmentDetailEntity GetLastChecker(string assessment_id, string check_object)
        {
            string sql = string.Format("select * from t_kpi_launch_detail where assessment_id = '{0}' and check_object = '{1}' and finished = 0 order by check_order asc", assessment_id, check_object);
            List<AssessmentDetailEntity> list = service.FindList(sql);
            if (list != null && list.Count > 0)
                return list[0];
            else return null;
        }


        public List<AssessmentDetailEntity> GetByAssessmentIdCheckObj(string assessment_id, string check_object)
        {
            string sql = string.Format("select * from t_kpi_launch_detail where assessment_id = '{0}' and check_object = '{1}'", assessment_id, check_object);
            return service.FindList(sql);
        }

        public AssessmentDetailEntity GetByAssessmentIdCheckerCheckObj(string assessment_id, string checker, string check_object)
        {
            string sql = string.Format("select * from t_kpi_launch_detail where assessment_id = '{0}' and checker = '{1}' and check_object = '{2}' order by check_order asc", assessment_id, checker, check_object);
            return service.FindList(sql)[0];
        }

        public List<AssessmentDetailEntity> GetListByAssessmentId(string assessment_id)
        {
            string sql = string.Format("select * from t_kpi_launch_detail where assessment_id='{0}'", assessment_id);
            return service.FindList(sql);
        }

        public bool IsLastChecker(string assessment_id)
        {
            string sql = string.Format("select Count(*) from t_kpi_launch_detail where assessment_id = '{0}' and finished = 0 ", assessment_id);
            int result = service.BasicQueryT<int>(sql);
            return result <= 1;//因为还会插入一个归档的任务 所以判断条件为<=1
        }

        public bool IsLastCheckerToObject(string assessment_id, string checker, string check_objct)
        {
            string sql = string.Format("select Count(*) from t_kpi_launch_detail where assessment_id = '{0}' and finished = 0 and checker != '{1}' and check_object = '{2}'", assessment_id, checker, check_objct);
            int result = service.BasicQueryT<int>(sql);
            return result == 0;
        }


        public AssessmentDetailEntity GetByAssessmentIdCheckOrderCheckObj(string assessment_id, string check_object, int check_order)
        {
            var expression = ExtLinq.True<AssessmentDetailEntity>();
            if (!string.IsNullOrEmpty(assessment_id))
            {
                expression = expression.And(r => r.assessment_id == assessment_id);
            }
            if (!string.IsNullOrEmpty(check_object))
            {
                expression = expression.And(r => r.check_object == check_object);
            }
            expression = expression.And(r => r.check_order == check_order);
            return service.FindEntity(expression);
        }

        public List<AssessmentDetailEntity> GetFirstCheckerList(string assessment_id)
        {
            string sql = string.Format("select * from t_kpi_launch_detail where check_order = 1 and assessment_id = '{0}' ", assessment_id);
            return service.FindList(sql).ToList();
        }

        public bool IsSelfCheck(string assessmentid, string checker, string checkobj)
        {
            string sql = string.Format("select count(*) from t_kpi_launch_detail where check_order = 1 and finished = '0' and assessment_id = '{0}' and checker = '{1}' and check_object = '{2}'", assessmentid, checker, checkobj);
            return service.BasicQueryT<int>(sql) > 0;
        }
    }
}