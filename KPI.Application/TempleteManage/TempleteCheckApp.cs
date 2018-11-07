using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Repository.IndicatorManage;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.IRepository;
using KPI.Code;
using System.Linq.Expressions;
using System;
using KPI.Repository.TempleteManage;
using KPI.Domain.Entity.TempleteManage;

namespace KPI.Application.IndicatorManage
{
    public class TempleteCheckApp
    {
        private ITempleteCheckRepository service = new TempleteCheckRepository();


        public List<TempleteCheckEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public TempleteCheckEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.id.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.id == keyValue);
            }
        }
        public void SubmitForm(TempleteCheckEntity TempleteCheckEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                TempleteCheckEntity.Modify(keyValue);
                service.Update(TempleteCheckEntity);
            }
            else
            {
                TempleteCheckEntity.Create();
                service.Insert(TempleteCheckEntity);
            }
        }

        public TempleteCheckEntity SubmitFormEx(TempleteCheckEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
            }
            return entity;
        }

        public TempleteCheckEntity GetTempleteCheckByTempleteCheckId(string templete_check_id)
        {
            var expression = ExtLinq.True<TempleteCheckEntity>();
            if (!string.IsNullOrEmpty(templete_check_id))
            {
                expression = expression.And(r => r.id == templete_check_id);
            }
            return service.FindEntity(expression);
        }

        public TempleteCheckEntity GetTempleteCheckByTempleteId(string templete_id)
        {
            var expression = ExtLinq.True<TempleteCheckEntity>();
            if (!string.IsNullOrEmpty(templete_id))
            {
                expression = expression.And(r => r.templete_id == templete_id);
            }
            return service.FindEntity(expression);
        }
    }
}
