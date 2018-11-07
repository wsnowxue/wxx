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
using KPI.Domain.Entity.TaskManage;
using KPI.Domain.Entity.SystemManage;

namespace KPI.Application.TaskManage
{
    public class BranchFinancialProductTaskDetailImportApp : ExcelImportBase
    {
        //经理室角色的主键
        private readonly string ManagerFamilyRoleId = Configs.GetValue("ManagerFamilyRoleId");

        public BranchFinancialProductTaskDetailImportApp()
        {
        }
        IRepositoryBase DbContext;

        public BranchFinancialProductTaskDetailImportApp(IRepositoryBase dbContext)
        {
            DbContext = dbContext;

        }
        private OrganizeApp organizeApp = new OrganizeApp();
        private UserApp userApp = new UserApp();
        ///private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();
        private BranchFinancialProductTaskDetailApp branchFinancialProductTaskDetailApp = new BranchFinancialProductTaskDetailApp();

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
                    if (!Validate.IsNumber(value))
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
            String errorReason = "【金融产品模板】不正确，请下载最新Excel模板、并按正确顺序上传！";
            string taskType = keyValue[1];
            ErrorItem item = new ErrorItem();

            //分公司
            if (headerList != null && headerList.Count > 0)
            {
                string[] rightHeaderList = Constant.FinancialProductTaskTempleteListColumn;
                for (int i = 0; i < headerList.Count; i++)
                {
                    if (!headerList[i].Equals(rightHeaderList[i]))
                    {
                        item.ErrorReson = errorReason;
                        ErrorList.Add(item);
                        return false;
                    }
                }
            }
            else
            {
                item.ErrorReson = errorReason;
                ErrorList.Add(item);
                return false;
            }

            return true;
        }
        protected override bool SaveData(DataTable dt, params string[] arr)
        {

            string taskId = arr[0];
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserEntity userEntity = new UserEntity();
                    String receivePersonId = "";
                    BranchFinancialProductTaskDetailEntity entity = new BranchFinancialProductTaskDetailEntity();
                    entity.id = Common.GuId();
                    entity.task_id = taskId;
                    OrganizeEntity organizeEntity = organizeApp.GetOrgByName(dt.Rows[i][0] + "");
                    if (organizeEntity != null)
                    {
                        entity.task_object = organizeEntity.F_Id;

                        //获取次分公司的经理管理室
                        userEntity = userApp.GetMFUserByOrgIdAndRoleId(organizeEntity.F_Id, ManagerFamilyRoleId);
                        if (userEntity != null)
                        {
                            receivePersonId = userEntity.F_Id;
                        }
                    }

                    entity.traffic = Convert.ToInt32(dt.Rows[i][1].ToInt());
                    entity.traditional_new_car = Convert.ToInt32(dt.Rows[i][2]);
                    entity.YiRong_loan = Convert.ToInt32(dt.Rows[i][3]);
                    entity.public_credit = Convert.ToInt32(dt.Rows[i][4]);
                    entity.car_loaner = Convert.ToInt32(dt.Rows[i][5]);
                    entity.finanical_leasing = Convert.ToInt32(dt.Rows[i][6]);
                    entity.second_hand_car = Convert.ToInt32(dt.Rows[i][7]);
                    entity.statue = 0;
                    entity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                    entity.create_time = DateTime.Now;
                    DbContext.Insert<BranchFinancialProductTaskDetailEntity>(entity);
                    
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

    }
}
