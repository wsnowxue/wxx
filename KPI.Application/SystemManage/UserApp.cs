
using KPI.Code;
using KPI.Domain.Entity.SystemManage;
using KPI.Domain.IRepository.SystemManage;
using KPI.Repository.SystemManage;
using System;
using System.Collections.Generic;
using KPI.Domain.ViewModel;
using System.Data;

namespace KPI.Application.SystemManage
{
    public class UserApp : ExcelImportBase
    {
        private IUserRepository service = new UserRepository();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private OrganizeApp organizeApp = new OrganizeApp();
        private RoleApp roleApp = new RoleApp();


        public List<UserEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<UserEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Account.Contains(keyword));
                expression = expression.Or(t => t.F_RealName.Contains(keyword));
                expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
            }
            expression = expression.And(t => t.F_Account != "admin");
            return service.FindList(expression, pagination);
        }
        public UserEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
            }
            else
            {
                userEntity.Create();
            }
            service.SubmitForm(userEntity, userLogOnEntity, keyValue);
        }
        public void UpdateForm(UserEntity userEntity)
        {
            service.Update(userEntity);
        }
        public UserEntity CheckLogin(string username, string password)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnApp.UpdateForm(userLogOnEntity);
                        return userEntity;
                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }

        public void ChangePwd(string username, string oldpassword, string newpassword)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(oldpassword.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(newpassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                        userLogOnApp.UpdateForm(userLogOnEntity);
                    }
                    else
                    {
                        throw new Exception("原密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }

        }


        public List<CheckerModel> GetListByOrgIdAndRoleId(string orgId, string roleId)
        {
            string sql = string.Format("select a.F_Id as id,a.F_RealName as name,b.F_FullName as organize_name,c.F_FullName as department_name,d.F_FullName as role_name from sys_user a left join sys_organize b on a.F_OrganizeId = b.F_Id left join sys_organize c on a.F_DepartmentId = c.F_Id left join sys_role d on a.F_RoleId = d.F_Id where a.F_OrganizeId = '{0}' and a.F_RoleId = '{1}'", orgId, roleId);
            return service.BasicQueryListT<CheckerModel>(sql);
        }

        public UserEntity GetUserByTelphone(string telphone)
        {
            var expression = ExtLinq.True<UserEntity>();
            if (!string.IsNullOrEmpty(telphone))
            {
                expression = expression.And(t => t.F_MobilePhone == telphone);
            }
            return service.FindEntity(telphone);
        }

        public UserEntity GetUserByTelphoneSql(string telphone)
        {
            string sql = string.Format("select * from sys_user where F_MobilePhone = '{0}'", telphone);
            return service.BasicQueryT<UserEntity>(sql);
        }

        public List<UserEntity> GetList()
        {
            string sql = string.Format("select * from sys_user ");
            return service.FindList(sql);
        }

        public UserEntity GetByPhone(string phone)
        {
            var expression = ExtLinq.True<UserEntity>();
            expression = expression.And(t => t.F_MobilePhone.Equals(phone));
            return service.FindEntity(expression);
        }

        public List<UserEntity> GetUserListByOrgId(string orgId)
        {
            string sql = string.Format("select * from sys_user where F_OrganizeId = '{0}'", orgId);
            return service.FindList(sql);
        }

        //获取某个分公司经理室

        public UserEntity GetMFUserByOrgIdAndRoleId(string orgId,string roleId)
        {
            string sql = string.Format(@"SELECT u.* FROM sys_user u
                                         LEFT JOIN sys_role r
                                         ON r.F_Id = u.F_RoleId
                                         WHERE u.F_OrganizeId = '{0}'
                                         AND r.F_Id = '{1}'", orgId, roleId);
            return service.BasicQueryT<UserEntity>(sql);
        }
        /// <summary>
        /// 查某分公司下所有客户经理角色
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<UserEntity> GetByOrgIdAndCMRoleId(string org_id,string role_id )
        {
            string sql = string.Format(@"SELECT  u.* FROM sys_user u
           LEFT JOIN sys_role r
           ON u.F_RoleId = r.F_Id
           WHERE r.F_Id = '{0}'
           AND u.F_OrganizeId = '{1}' order by u.F_Id", role_id, org_id);
           return service.FindList(sql);
        }

        protected override bool CheckDataTable(DataTable dt, DataRow dr, ErrorItem item)
        {
            return true;
        }

        protected override bool CheckDataRow(DataRow dr, ErrorItem item, string[] keyValue)
        {
            OrganizeEntity organizeEntity = new OrganizeEntity();
            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                if (i == 0)//姓名
                {
                    string custName = dr.ItemArray[0] + "";
                    if (string.IsNullOrEmpty(custName))
                    {
                        item.ErrorReson = "姓名不能为空";
                        return false;
                    }
                    
                }
                if (i == 2)//手机号
                {
                    string phone = dr.ItemArray[2] + "";
                    if (string.IsNullOrEmpty(phone))
                    {
                        item.ErrorReson = "手机号不能为空";
                        return false;
                    }
                   
                }
                if (i == 4)//职务
                {
                    string roleName = dr.ItemArray[4] + "";
                    if (string.IsNullOrEmpty(roleName))
                    {
                        item.ErrorReson = "职务不能为空";
                        return false;
                    }
                    if (roleApp.GetByFullName(roleName, 1) == null)
                    {
                        item.ErrorReson = "“" + roleName + "”不存在";
                        return false;
                    }
                }
                else if (i == 5)//性别
                {
                    string sex = dr.ItemArray[5] + "";
                    if (string.IsNullOrEmpty(sex))
                    {
                        item.ErrorReson = "性别不能为空";
                        return false;
                    }
                }
                
                else if (i == 7)//所属分公司
                {
                    string orgName = dr.ItemArray[7] + "";
                    if (string.IsNullOrEmpty(orgName))
                    {
                        item.ErrorReson = "所属分公司不能为空";
                        return false;
                    }
                    organizeEntity = organizeApp.GetOrgByName(orgName);
                    if (organizeEntity == null)
                    {
                        item.ErrorReson = "所属分公司:“" + orgName + "”不存在";
                        return false;
                    }
                }
                else if (i == 6)//所属部门
                {
                    string departMentName = dr.ItemArray[6] + "";
                    if (string.IsNullOrEmpty(departMentName))
                    {
                        item.ErrorReson = "所属部门不能为空";
                        return false;
                    }
                    if (organizeApp.GetByParentIdAndFullName(organizeEntity.F_Id, departMentName) == null)
                    {
                        item.ErrorReson = "所属部门:“" + departMentName + "”不存在";
                        return false;
                    }
                }
            }
            return true;
        }
        protected override bool CheckHeader(List<String> headerList, List<ErrorItem> ErrorList, string[] keyValue)
        {
            String errorReason = "【分公司客户经理模板】不正确，请使用正确Excel模板！";
            
            ErrorItem item = new ErrorItem();

            if (headerList != null && headerList.Count > 0)
            {
                string[] rightHeaderList = Constant.CustomerManagerTempleteListColumn;

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
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserEntity userEntity = new UserEntity();
                    UserLogOnEntity userLogOnEntity = new UserLogOnEntity();
                    userEntity = GetByPhone(Convert.ToString(dt.Rows[i][2]));
                    if (userEntity == null)
                    {
                        //添加原来没有的客户经理
                        userEntity.F_Account = Convert.ToString(dt.Rows[i][2]);
                        userEntity.F_RealName = Convert.ToString(dt.Rows[i][0]);
                        userEntity.F_Gender = Convert.ToString(dt.Rows[i][5]).Equals("男");
                        userEntity.F_MobilePhone = Convert.ToString(dt.Rows[i][2]);
                        OrganizeEntity organizeEntity = organizeApp.GetOrgByName(Convert.ToString(dt.Rows[i][7]));
                        if (organizeEntity != null)
                        {
                            userEntity.F_OrganizeId = organizeEntity.F_Id;//根据分公司名称查找对应org
                        }
                        OrganizeEntity department = organizeApp.GetByParentIdAndFullName(organizeEntity.F_Id, Convert.ToString(dt.Rows[i][6]));
                        if (organizeEntity != null)
                        {
                            userEntity.F_DepartmentId = department.F_Id;//查找该分公司下的此部门
                        }
                        String departmentStr = Convert.ToString(dt.Rows[i][6]).Substring(0, Convert.ToString(dt.Rows[i][6]).IndexOf("组"));//第一个“组”之前的
                        String roleFullName = Convert.ToString(dt.Rows[i][7]).Replace("分公司", "") + "-" + departmentStr + "-" + Convert.ToString(dt.Rows[i][4]);
                        RoleEntity dutyEntity = roleApp.GetByFullName(roleFullName, 2);
                        //if (roleEntity == null) {
                        //    //角色等于空  就添加
                        //    roleEntity.F_FullName=roleFullName;
                        //    roleEntity.F_OrganizeId= department.F_Id;
                        //    roleEntity.F_Category = 2;
                        //    roleEntity.F_EnCode = "KHJL";
                        //    roleEntity.F_FullName = roleFullName;
                        //    roleEntity.F_AllowEdit = false;
                        //    roleEntity.F_AllowDelete = false;
                        //    roleEntity.F_SortCode = 17;
                        //    roleEntity.F_EnabledMark = true;

                        //    roleApp.SubmitForm(roleEntity, "");
                        //}
                        if (dutyEntity != null)
                        {
                            userEntity.F_DutyId = dutyEntity.F_Id;//根据“安徽-六安-客户经理”拼装查找
                        }
                        RoleEntity roleEntity = roleApp.GetByFullName(Convert.ToString(dt.Rows[i][4]), 1);
                        if (roleEntity != null)
                        {
                            userEntity.F_RoleId = roleEntity.F_Id;
                        }
                        userEntity.F_IsAdministrator = false;
                        userEntity.F_EnabledMark = true;

                        userLogOnEntity.F_UserPassword = "1";
                        SubmitForm(userEntity, userLogOnEntity, "");
                    }
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
