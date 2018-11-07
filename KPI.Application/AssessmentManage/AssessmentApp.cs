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

namespace KPI.Application.AssessmentManage
{
    public class AssessmentApp
    {
        private IAssessmentRepository service = new AssessmentRepository();

        public List<AssessmentEntity> GetItemList(string assessment_name, string assessment_sponser, int? assessment_statue, Pagination pagination)
        {
            var expression = ExtLinq.True<AssessmentEntity>();
            if (!string.IsNullOrEmpty(assessment_name))
            {
                expression = expression.And(r => r.assessment_name.Contains(assessment_name));
            }
            if (!string.IsNullOrEmpty(assessment_sponser))
            {
                expression = expression.And(r => r.assessment_sponsor.Contains(assessment_sponser));
            }
            if (assessment_statue.HasValue)
                expression = expression.And(r => r.assessment_statue == assessment_statue.Value);
            if (pagination.rows == 0) return service.IQueryable(expression).ToList();
            return service.FindList(expression, pagination);
        }

        public List<AssessmentEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.create_time).ToList();
        }
        public AssessmentEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.id == keyValue);
        }
        public void SubmitForm(AssessmentEntity assessmentEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                assessmentEntity.Modify(keyValue);
                service.Update(assessmentEntity);
            }
            else
            {
                assessmentEntity.Create();
                service.Insert(assessmentEntity);
            }
        }

        public AssessmentEntity SubmitFormEx(AssessmentEntity assessmentEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                assessmentEntity.Modify(keyValue);
                service.Update(assessmentEntity);
            }
            else
            {
                assessmentEntity.Create();
                service.Insert(assessmentEntity);
            }
            return assessmentEntity;
        }

        public List<AssessmentOverviewModel> GetAssessmentOverview(string assessment_name, string assessment_sponsor, int? assessment_statue, Pagination pagination, int assessment_type, string organize_id = "")
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select a.id,a.assessment_name,a.filing_people,b.templete_name,a.start_time,a.end_time,c.F_RealName as assessment_sponsor_name,a.assessment_sponsor as assessment_sponsor_id,a.assessment_count,a.assessment_statue,a.create_time ");
            sqlBuilder.Append("from t_kpi_launch a left join t_kpi_templete b on a.templete_id = b.id left join sys_user c on a.assessment_sponsor = c.F_Id ");
            sqlBuilder.Append("LEFT JOIN sys_organize d ON c.F_OrganizeId = d.F_Id ");
            sqlBuilder.AppendFormat("where assessment_type = {0} ", assessment_type);
            if (!string.IsNullOrEmpty(assessment_name))
                sqlBuilder.AppendFormat(" and a.assessment_name like '%{0}%' ", assessment_name);
            if (!string.IsNullOrEmpty(assessment_sponsor))
                sqlBuilder.AppendFormat(" and c.F_RealName ='{0}'", assessment_sponsor);
            if (assessment_statue.HasValue)
                sqlBuilder.AppendFormat(" and a.assessment_statue ={0} ", assessment_statue.Value);
            if (!string.IsNullOrEmpty(organize_id))
                sqlBuilder.AppendFormat(" AND d.F_Id = '{0}' ", organize_id);
            IEnumerable<AssessmentOverviewModel> retData = null;
            if (pagination == null || pagination.rows == 0)
                retData = service.BasicQueryListT<AssessmentOverviewModel>(sqlBuilder.ToString());
            else
                retData = service.FindListBySql<AssessmentOverviewModel>(sqlBuilder.ToString(), pagination);
            return retData.ToList();
        }

        public AssessmentEntity GetByAssessmentId(string assessment_id)
        {
            var expression = ExtLinq.True<AssessmentEntity>();
            if (!string.IsNullOrEmpty(assessment_id))
            {
                expression = expression.And(r => r.id == assessment_id);
            }
            return service.FindEntity(expression);
        }


        public bool IsExists(string assessment_name)
        {
            string sql = string.Format("select * from t_kpi_launch where assessment_name = '{0}'", assessment_name);
            return service.FindList(sql).Count > 0;
        }
    }
}
