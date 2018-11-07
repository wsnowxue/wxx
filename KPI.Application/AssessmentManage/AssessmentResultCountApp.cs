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
using KPI.Data;
using KPI.Application.SystemManage;
using KPI.Application.IndicatorManage;



namespace KPI.Application.AssessmentManage
{
    public class AssessmentResultCountApp
    {
        private IAssessmentResultCountRepository service = new AssessmentResultCountRepository();

        public List<AssessmentResultCountEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.create_time).ToList();
        }
        public AssessmentResultCountEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.id == keyValue);
        }
        public void SubmitForm(AssessmentResultCountEntity assessmentresultcountEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                assessmentresultcountEntity.Modify(keyValue);
                service.Update(assessmentresultcountEntity);
            }
            else
            {
                assessmentresultcountEntity.Create();
                service.Insert(assessmentresultcountEntity);
            }
        }

        public AssessmentResultCountEntity GetByAssessmentIdCheckerCheckObject(string assessment_id, string checker, string check_object)
        {
            var expression = ExtLinq.True<AssessmentResultCountEntity>();
            if (!string.IsNullOrEmpty(assessment_id))
            {
                expression = expression.And(r => r.assessment_id == assessment_id);
            }
            if (!string.IsNullOrEmpty(checker))
            {
                expression = expression.And(r => r.checker == checker);
            }
            if (!string.IsNullOrEmpty(check_object))
            {
                expression = expression.And(r => r.check_object == check_object);
            }
            return service.FindEntity(expression);
        }

        public List<AssessmentResultCountEntity> GetByAssessmentIdCheckObject(string assessment_id, string check_object)
        {
            string sql = string.Format("select * from t_kpi_result where assessment_id = '{0}' and check_object = '{1}'", assessment_id, check_object);
            return service.FindList(sql);
        }

        public List<AssessmentResultCountEntity> GetByAssessmentId(string assessment_id)
        {
            string sql = string.Format("select * from t_kpi_result where assessment_id = '{0}'", assessment_id);
            return service.FindList(sql);
        }

        public List<AssessmentResultCountEntity> GetMonthAssessment(string check_object, string month)
        {
            string timeStr = DateTime.Now.ToString("yyyy-") + month.PadLeft(2, '0');
            string sql = string.Format("SELECT a.* FROM t_kpi_result a LEFT JOIN t_kpi_launch b ON a.assessment_id = b.id WHERE a.check_object = '{0}' AND b.start_time LIKE '%{1}%'", check_object, timeStr);
            return service.FindList(sql);
        }

    }
}