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
    public class BranchAssessmentImportApp : ExcelImportBase
    {
        IRepositoryBase DbContext;

        public BranchAssessmentImportApp(IRepositoryBase dbContext)
        {
            DbContext = dbContext;
        }

        private OrganizeApp organizeApp = new OrganizeApp();
        private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();
        private AssessmentResultApp assessmentResultApp = new AssessmentResultApp();

        protected override bool CheckDataTable(DataTable dt, DataRow dr, ErrorItem item)
        {
            return true;
        }

        protected override bool CheckDataRow(DataRow dr, ErrorItem item, string[] keyValue)
        {
            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                if (i == 0)
                {
                    string branchName = dr.ItemArray[0] + "";
                    if (string.IsNullOrEmpty(branchName))
                    {
                        item.ErrorReson = "分公司名称不能为空";
                        return false;
                    }
                    if (organizeApp.GetOrgByName(branchName) == null)
                    {
                        item.ErrorReson = "“" + branchName + "”该分公司不存在";
                        return false;
                    }
                }
                else
                {
                    string value = dr.ItemArray[i] + "";
                    if (string.IsNullOrEmpty(value))
                    {
                        item.ErrorReson = "完成量不能为空";
                        return false;
                    }
                    if (!(Validate.IsNumber(value) || Validate.IsDecimal(value)))
                    {
                        item.ErrorReson = "指标量必须是数字";
                        return false;
                    }
                }

            }
            return true;
        }

        protected override bool CheckHeader(List<String> headerList, List<ErrorItem> ErrorList, string[] keyValue)
        {
            return true;
        }

        protected override bool SaveData(DataTable dt, params string[] arr)
        {
            string assessmentId = arr[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    AssessmentResultEntity entity = null;
                    string indicator_id = indicatorsDefineApp.GetIndicatorByName(dt.Columns[j].ColumnName + "").id;
                    string orgId = organizeApp.GetOrgByName(dt.Rows[i][0] + "").F_Id;
                    entity = assessmentResultApp.GetByAssessmentIdObjIndicatorId(assessmentId, orgId, indicator_id);
                    if (entity == null)//如果是第一次考评
                    {
                        entity = new AssessmentResultEntity();
                        entity.id = Common.GuId();
                        entity.assessment_id = assessmentId;
                        entity.check_object = orgId;
                        entity.indicator_id = indicator_id;
                        entity.indicator_value = double.Parse(dt.Rows[i][j] + "");
                        entity.create_time = DateTime.Now;
                        entity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        DbContext.Insert<AssessmentResultEntity>(entity);
                    }
                    else //如果是重新发起的考评
                    {
                        entity.indicator_value = double.Parse(dt.Rows[i][j] + "");
                        DbContext.Update<AssessmentResultEntity>(entity);
                    }
                }
            }
            return true;
        }

    }
}
