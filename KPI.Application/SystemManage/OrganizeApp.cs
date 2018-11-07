
using KPI.Code;
using KPI.Domain.Entity.SystemManage;
using KPI.Domain.IRepository.SystemManage;
using KPI.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KPI.Application.SystemManage
{
    public class OrganizeApp
    {
        private IOrganizeRepository service = new OrganizeRepository();

        public List<OrganizeEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }
        public OrganizeEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                organizeEntity.Modify(keyValue);
                service.Update(organizeEntity);
            }
            else
            {
                organizeEntity.Create();
                service.Insert(organizeEntity);
            }
        }

        public List<OrganizeEntity> GetListByParentId(string parentId)
        {
            var expression = ExtLinq.True<OrganizeEntity>();
            expression = expression.And(t => t.F_ParentId == parentId);
            return service.IQueryable(expression).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public OrganizeEntity GetOrgByName(string orgName)
        {
            var expression = ExtLinq.True<OrganizeEntity>();
            expression = expression.And(t => t.F_FullName.Equals(orgName));
            return service.FindEntity(expression);
        }

        public OrganizeEntity GetOrgById(string orgId)
        {
            var expression = ExtLinq.True<OrganizeEntity>();
            expression = expression.And(t => t.F_Id == orgId);
            return service.FindEntity(expression);
        }

        //获取除了中安总部外的所有分公司
        public List<OrganizeEntity> GetListByParentIdOrderById(string parentId,string CompanyId)
        {
            var expression = ExtLinq.True<OrganizeEntity>();
            expression = expression.And(t => t.F_ParentId == parentId);
            expression = expression.And(t => t.F_Id != CompanyId);
            return service.IQueryable(expression).OrderBy(t => t.F_Id).ToList();
        }

        //获取除了中安总部外的所有分公司
        public OrganizeEntity GetByParentIdAndFullName(string parentId, string FullName)
        {
            var expression = ExtLinq.True<OrganizeEntity>();
            expression = expression.And(t => t.F_ParentId.Equals(parentId));
            expression = expression.And(t => t.F_FullName.Equals(FullName));
            return service.FindEntity(expression);
        }
        
    }
}
