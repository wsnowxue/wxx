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

namespace KPI.Application.TempleteManage
{
    public class TempleteCompostionApp
    {
        private ITempleteCompostionRepository service = new TempleteCompostionRepository();

        #region 带起始索引、查询条数的查询
        //public List<TempleteCompostionEntity> GetItemList(string dimension_name, string pagination)
        //{
        //    Pagination p = Json.ToObject<Pagination>(pagination);
        //    if (p.rows == 0) {
        //        //分页rows=0 获取全部
        //        return GetList();
        //    }
        //    var expression = ExtLinq.True<TempleteCompostionEntity>();
        //    if (!string.IsNullOrEmpty(dimension_name))
        //    {
        //        expression = expression.And(r => r.dimension_name.Contains(dimension_name));
        //    }
        //    return service.FindList(expression, p);
        //}
        #endregion

        public List<TempleteCompostionEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public TempleteCompostionEntity GetForm(string keyValue)
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
        public void SubmitForm(TempleteCompostionEntity templeteCompostionEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                templeteCompostionEntity.Modify(keyValue);
                service.Update(templeteCompostionEntity);
            }
            else
            {
                templeteCompostionEntity.Create();
                service.Insert(templeteCompostionEntity);
            }
        }

        public void DeleteIndicatorByTempleteId(string templete_id)
        {
            var expression = ExtLinq.True<TempleteCompostionEntity>();
            if (!string.IsNullOrEmpty(templete_id))
            {
                expression = expression.And(r => r.templete_id.Equals(templete_id));
            }
            service.Delete(expression);
            //StringBuilder sqlBuilder = new StringBuilder();
            //sqlBuilder.Append(@"DELETE FROM t_kpi_templete_compostion  ");
            //if (!string.IsNullOrEmpty(templete_id))
            //{
            //    sqlBuilder.AppendFormat("WHERE templete_id= '{0}'", templete_id);
            //}
        }

        public List<TempleteCompostionEntity> GetByTempleteId(string templete_id)
        {
            string sql = string.Format(@"select * FROM t_kpi_templete_compostion WHERE templete_id= '{0}'", templete_id);
            return service.FindList(sql);
        }
    }
}
