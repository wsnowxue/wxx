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
    public class YearlyTaskDetailImportApp : ExcelImportBase
    {
        //经理室角色的主键
        private readonly string ManagerFamilyRoleId = Configs.GetValue("ManagerFamilyRoleId");
        public YearlyTaskDetailImportApp()
        {
        }

        IRepositoryBase DbContext;

        public YearlyTaskDetailImportApp(IRepositoryBase dbContext)
        {
            DbContext = dbContext;

        }
        private OrganizeApp organizeApp = new OrganizeApp();
        private UserApp userApp = new UserApp();
        ///private IndicatorsDefineApp indicatorsDefineApp = new IndicatorsDefineApp();
        private YearlyTaskDetailApp yearlyTaskDetailApp = new YearlyTaskDetailApp();

        protected override bool CheckDataTable(DataTable dt, DataRow dr, ErrorItem item)
        {
            return true;
        }

        protected override bool CheckDataRow(DataRow dr, ErrorItem item, string[] keyValue)
        {
            string taskType = keyValue[1];
            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                if (i == 0)
                {
                    if (taskType.Equals("0"))
                    {
                        //分公司
                        string branchName = dr.ItemArray[0] + "";
                        if (string.IsNullOrEmpty(branchName))
                        {
                            item.ErrorReson = "分公司名称不能为空";
                            return false;
                        }
                        if (organizeApp.GetOrgByName(branchName) == null)
                        {
                            item.ErrorReson = "“" + branchName + "”不存在";
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // 个人
                            string userName = dr.ItemArray[0] + "";
                            string userPhone = dr.ItemArray[14] + "";

                            if (string.IsNullOrEmpty(userName))
                            {
                                item.ErrorReson = "员工姓名不能为空";
                                return false;
                            }
                            if (string.IsNullOrEmpty(userPhone))
                            {
                                item.ErrorReson = "员工电话号码不能为空";
                                return false;
                            }
                            if (userApp.GetByPhone(userPhone) == null)
                            {
                                item.ErrorReson = "“" + userName + "”(phone:" + userPhone + ")不存在";
                                return false;
                            }
                        }
                        catch
                        {
                            item.ErrorReson = "请下载使用最新正确的【个人】全年任务指标数据录入模板！";
                            return false;
                        }
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
            String errorReason = "【全年任务模板】不正确，请下载最新Excel模板、并按正确顺序上传！";
            string taskType = keyValue[1];
            ErrorItem item = new ErrorItem();

            if (headerList != null && headerList.Count > 0)
            {
                string[] rightHeaderList = taskType.Equals("0") ? Constant.YearlyTaskTempleteCompanyListColumn : Constant.YearlyTaskTempletePersonListColumn;

                if (rightHeaderList.Length != headerList.Count)
                {
                    item.ErrorReson = errorReason;
                    ErrorList.Add(item);
                    return false;
                }
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
            string taskType = arr[1];
            //string endDate = arr[2];
            
            try
            {
                UserEntity userEntity = new UserEntity();
                String receivePersonId = "";
                if (taskType.Equals("1"))
                {

                    //个人  把经理室的角色给添加上
                    userEntity = userApp.GetMFUserByOrgIdAndRoleId(OperatorProvider.Provider.GetCurrent().CompanyId, ManagerFamilyRoleId);
                    if (userEntity != null)
                    {
                        receivePersonId = userEntity.F_Id;
                    }
                    if (!String.IsNullOrEmpty(receivePersonId))
                    {
                        SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                        selfTaskDetailEntity.id = Common.GuId();
                        selfTaskDetailEntity.task_type = 5;//待分发
                        selfTaskDetailEntity.person = receivePersonId;
                        selfTaskDetailEntity.task_statue = 2;//0 未激活  1待完成  2已完成
                        selfTaskDetailEntity.task_object = taskId;
                        selfTaskDetailEntity.statue = 0;//0未删除  1已删除
                        selfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        selfTaskDetailEntity.create_time = DateTime.Now;
                        DbContext.Insert<SelfTaskDetailEntity>(selfTaskDetailEntity);
                    }

                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    receivePersonId = "";
                    userEntity = new UserEntity();
                    YearlyTaskDetailEntity entity = new YearlyTaskDetailEntity();
                    entity.id = Common.GuId();
                    entity.task_id = taskId;

                    if (taskType.Equals("0"))
                    {
                        //公司
                        OrganizeEntity organizeEntity = organizeApp.GetOrgByName(Convert.ToString(dt.Rows[i][0]));
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
                    }
                    else
                    {
                        //个人

                        userEntity = userApp.GetByPhone(Convert.ToString(dt.Rows[i][14]));
                        if (userEntity != null)
                        {
                            entity.task_object = userEntity.F_Id;
                        }
                        receivePersonId = userEntity.F_Id;

                    }
                    entity.task_yearly = Convert.ToInt32(dt.Rows[i][1]);//indicatorsDefineApp.GetForm(dt.Rows[0][j] + "").id;
                    entity.start_date = null;// Convert.ToDateTime(startDate);
                    entity.end_date = null;//Convert.ToDateTime(endDate);
                    entity.task_Jan = Convert.ToInt32(dt.Rows[i][2]);
                    entity.task_Feb = Convert.ToInt32(dt.Rows[i][3]);
                    entity.task_Mar = Convert.ToInt32(dt.Rows[i][4]);
                    entity.task_Apr = Convert.ToInt32(dt.Rows[i][5]);
                    entity.task_May = Convert.ToInt32(dt.Rows[i][6]);
                    entity.task_Jun = Convert.ToInt32(dt.Rows[i][7]);
                    entity.task_Jul = Convert.ToInt32(dt.Rows[i][8]);
                    entity.task_Aug = Convert.ToInt32(dt.Rows[i][9]);
                    entity.task_Sep = Convert.ToInt32(dt.Rows[i][10]);
                    entity.task_Oct = Convert.ToInt32(dt.Rows[i][11]);
                    entity.task_Nov = Convert.ToInt32(dt.Rows[i][12]);
                    entity.task_Dec = Convert.ToInt32(dt.Rows[i][13]);
                    entity.statue = 0;
                    entity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                    entity.create_time = DateTime.Now;
                    DbContext.Insert<YearlyTaskDetailEntity>(entity);

                    //为分公司或者个人添加自己的任务
                    #region 添加个人的任务
                    if (!String.IsNullOrEmpty(receivePersonId))
                    {
                        SelfTaskDetailEntity selfTaskDetailEntity = new SelfTaskDetailEntity();
                        selfTaskDetailEntity.id = Common.GuId();
                        selfTaskDetailEntity.task_type = 5;//待分发
                        selfTaskDetailEntity.person = receivePersonId;
                        selfTaskDetailEntity.task_statue = 0;//0 未激活  1待完成  2已完成
                        selfTaskDetailEntity.task_object = taskId;
                        selfTaskDetailEntity.statue = 0;//0未删除  1已删除
                        selfTaskDetailEntity.creator_user_id = OperatorProvider.Provider.GetCurrent().UserId;
                        selfTaskDetailEntity.create_time = DateTime.Now;
                        DbContext.Insert<SelfTaskDetailEntity>(selfTaskDetailEntity);
                    }

                    #endregion

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


