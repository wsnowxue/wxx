
using KPI.Application.SystemManage;
using KPI.Code;
using KPI.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KPI.Domain.ViewModel;


namespace KPI.Web.Areas.SystemManage.Controllers
{
    public class UserController : ControllerBase
    {
        //分公司考核管理员所属机构在系统机构表中的主键
        private readonly string CompanyId = Configs.GetValue("CompanyId");
        //考核管理员角色主键
        private readonly string BranchAssessmentManagerRoleId = Configs.GetValue("BranchAssessmentManagerRoleId");
        private readonly string PersonalAssessmentManagerRoleId = Configs.GetValue("PersonalAssessmentManagerRoleId");

        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private OrganizeApp organizeApp = new OrganizeApp();
        private RoleApp roleApp = new RoleApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = userApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            userApp.SubmitForm(userEntity, userLogOnEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            userApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            userLogOnApp.RevisePassword(userPassword, keyValue);
            return Success("重置密码成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.F_Id = keyValue;
            userEntity.F_EnabledMark = false;
            userApp.UpdateForm(userEntity);
            return Success("账户禁用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.F_Id = keyValue;
            userEntity.F_EnabledMark = true;
            userApp.UpdateForm(userEntity);
            return Success("账户启用成功。");
        }

        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ChangePwd(string username, string oldpassword, string newpassword)
        {
            userApp.ChangePwd(username, oldpassword, newpassword);
            return Success("密码修改成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCompanyCheckerList()
        {
            List<CheckerModel> list = userApp.GetListByOrgIdAndRoleId(CompanyId, BranchAssessmentManagerRoleId);
            return Success("操作成功。", list, list.Count);
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBranchCheckerList()
        {
            List<CheckerModel> list = userApp.GetListByOrgIdAndRoleId(OperatorProvider.Provider.GetCurrent().CompanyId, PersonalAssessmentManagerRoleId);
            return Success("操作成功。", list, list.Count);
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeJson(string organize_id)
        {
            var treeList = new List<TreeViewModel>();
            //获取所有分公司和部门信息
            var data = organizeApp.GetList();
            //分公司
            OrganizeEntity company = organizeApp.GetOrgById(organize_id);
            TreeViewModel tree = new TreeViewModel();
            List<OrganizeEntity> departmentList = data.FindAll(t => t.F_ParentId == company.F_Id);
            bool hasChildren = departmentList.Count == 0 ? false : true;
            tree.id = company.F_Id;
            tree.text = company.F_FullName;
            tree.value = company.F_EnCode;
            tree.parentId = company.F_ParentId;
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren = hasChildren;
            tree.nodeDepth = 1;
            treeList.Add(tree);
            //部门
            if (hasChildren)
            {
                for (int i = 0; i < departmentList.Count; i++)
                {
                    OrganizeEntity department = departmentList[i];
                    TreeViewModel departmentTree = new TreeViewModel();
                    var roleData = roleApp.GetDutyList();
                    List<RoleEntity> dutyList = roleData.FindAll(t => t.F_OrganizeId == department.F_Id && t.F_Category == 2);
                    hasChildren = dutyList.Count == 0 ? false : true;
                    departmentTree.id = department.F_Id;
                    departmentTree.text = department.F_FullName;
                    departmentTree.value = department.F_EnCode;
                    departmentTree.parentId = company.F_Id;
                    departmentTree.isexpand = true;
                    departmentTree.complete = true;
                    departmentTree.hasChildren = hasChildren;
                    departmentTree.nodeDepth = 2;
                    treeList.Add(departmentTree);
                    //岗位
                    if (hasChildren)
                    {
                        for (int j = 0; j < dutyList.Count; j++)
                        {
                            RoleEntity duty = dutyList[j];
                            TreeViewModel dutyTree = new TreeViewModel();
                            var userData = userApp.GetList();
                            List<UserEntity> userList = userData.FindAll(t => t.F_DutyId == duty.F_Id && t.F_OrganizeId == company.F_Id && t.F_DepartmentId == department.F_Id);
                            hasChildren = userList.Count == 0 ? false : true;
                            dutyTree.id = duty.F_Id;
                            dutyTree.text = duty.F_FullName;
                            dutyTree.value = duty.F_EnCode;
                            dutyTree.parentId = department.F_Id;
                            dutyTree.isexpand = true;
                            dutyTree.complete = true;
                            dutyTree.hasChildren = hasChildren;
                            dutyTree.nodeDepth = 3;
                            treeList.Add(dutyTree);
                            if (hasChildren)
                            {
                                for (int k = 0; k < userList.Count; k++)
                                {
                                    UserEntity user = userList[k];
                                    TreeViewModel userTree = new TreeViewModel();
                                    hasChildren = false;
                                    userTree.id = user.F_Id;
                                    userTree.text = user.F_RealName;
                                    userTree.value = user.F_Id;
                                    userTree.parentId = duty.F_Id;
                                    userTree.isexpand = true;
                                    userTree.complete = true;
                                    userTree.hasChildren = hasChildren;
                                    userTree.nodeDepth = 4;
                                    treeList.Add(userTree);
                                }
                            }
                        }
                    }
                }

            }
            return Content(treeList.TreeViewJson(company.F_ParentId));
        }

    }
}
