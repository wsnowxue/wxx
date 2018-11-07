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
    public class AssessmentResultApp
    {
        private IAssessmentResultRepository service = new AssessmentResultRepository();

        public List<AssessmentResultEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.create_time).ToList();
        }
        public AssessmentResultEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.id == keyValue);
        }
        public void SubmitForm(AssessmentResultEntity assessmentresultEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                assessmentresultEntity.Modify(keyValue);
                service.Update(assessmentresultEntity);
            }
            else
            {
                assessmentresultEntity.Create();
                service.Insert(assessmentresultEntity);
            }
        }

        public List<AssessmentIndicatorsValueModel> GetAllIndicatorsResult(string assessment_id, string checker, string check_object)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select a.id,a.assessment_id,b.assessment_name,a.check_object,a.checker,a.indicator_id,c.indicator_name,a.indicator_value from t_kpi_result_detail a left join t_kpi_launch b on a.assessment_id = b.id left join t_indicators_define c on a.indicator_id = c.id ");
            sqlBuilder.AppendFormat("where a.assessment_id = '{0}' and a.check_object = '{1}' ", assessment_id, check_object);
            if (!string.IsNullOrEmpty(checker))
                sqlBuilder.AppendFormat(" and a.checker = '{0}'", checker);
            return service.BasicQueryListT<AssessmentIndicatorsValueModel>(sqlBuilder.ToString());
        }


        public List<AssessmentTempleteResultModel> GetAssessmentTempleteResultOverview(string assessment_id, string check_object)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT DISTINCT a.id AS id,a.assessment_name AS assessment_name,a.templete_id AS templete_id,b.check_object AS check_object, ");
            sqlBuilder.Append("c.dimension_id AS dimension_id,d.dimension_name AS dimension_name,c.detail_id AS dimension_detail_id,e.detail_name AS dimension_detail_name, ");
            sqlBuilder.Append("e.formule AS formule,e.method_id AS method_id,f.check_method_proc as check_method_proc,c.base_score AS base_score,f.check_method_name AS check_method ");
            sqlBuilder.Append("FROM t_kpi_launch a  ");
            sqlBuilder.Append("LEFT JOIN t_kpi_launch_detail b ON a.id = b.assessment_id ");
            sqlBuilder.Append("LEFT JOIN t_kpi_templete_compostion c ON a.templete_id = c.templete_id ");
            sqlBuilder.Append("LEFT JOIN t_kpi_dimension d ON c.dimension_id = d.id ");
            sqlBuilder.Append("LEFT JOIN t_kpi_dimension_detail e ON c.detail_id = e.id ");
            sqlBuilder.Append("LEFT JOIN t_check_method f ON e.method_id = f.id ");
            sqlBuilder.AppendFormat("WHERE a.id = '{0}' AND b.check_object = '{1}'", assessment_id, check_object);
            IEnumerable<AssessmentTempleteResultModel> retData = null;
            retData = service.BasicQueryListT<AssessmentTempleteResultModel>(sqlBuilder.ToString());
            return retData.ToList();
        }

        public AssessmentResultEntity GetByAssessmentIdIndicatorId(string assessment_id, string indicator_id)
        {
            var expression = ExtLinq.True<AssessmentResultEntity>();
            if (!string.IsNullOrEmpty(assessment_id))
            {
                expression = expression.And(r => r.assessment_id == assessment_id);
            }
            if (!string.IsNullOrEmpty(indicator_id))
            {
                expression = expression.And(r => r.indicator_id == indicator_id);
            }
            return service.FindEntity(expression);
        }

        public AssessmentResultEntity GetByAssessmentIdObjIndicatorId(string assessment_id,string check_object, string indicator_id)
        {
            var expression = ExtLinq.True<AssessmentResultEntity>();
            if (!string.IsNullOrEmpty(assessment_id))
            {
                expression = expression.And(r => r.assessment_id== assessment_id);
            }
            if (!string.IsNullOrEmpty(check_object))
            {
                expression = expression.And(r => r.check_object == check_object);
            }
            if (!string.IsNullOrEmpty(indicator_id))
            {
                expression = expression.And(r => r.indicator_id == indicator_id);
            }
            return service.FindEntity(expression);
        }
    }
}