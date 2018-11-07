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

namespace KPI.Application.IndicatorManage
{
    public class DimensionApp
    {
        private IDimensionRepository service = new DimensionRepository();

        #region 带起始索引、查询条数的查询
        public List<DimensionEntity> GetItemList(string dimension_name,int? statue, Pagination pagination)
        {
           
            if (pagination.rows == 0|| pagination==null) {
                //分页rows=0 获取全部
                return GetDimendionByStatue(statue);
            }
            var expression = ExtLinq.True<DimensionEntity>();
            if (!string.IsNullOrEmpty(dimension_name))
            {
                expression = expression.And(r => r.dimension_name.Contains(dimension_name));
            }
            if (statue!=null)
            {
                expression = expression.And(r => r.statue== statue);
            }
            return service.FindList(expression, pagination);
        }
        #endregion

        public List<DimensionEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public DimensionEntity GetForm(string keyValue)
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
        public void SubmitForm(DimensionEntity dimensionEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                dimensionEntity.Modify(keyValue);
                service.Update(dimensionEntity);
            }
            else
            {
                dimensionEntity.Create();
                service.Insert(dimensionEntity);
            }
        }

        public List<DimensionEntity> GetDimendionByStatue(int? statue)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT * FROM t_kpi_dimension ");
            if (statue != null)
            {
                sqlBuilder.AppendFormat("WHERE statue= {0}", statue);
            }
            return service.FindList(sqlBuilder.ToString());
        }

        public bool IsExists(string dimension_name)
        {
            string sql = string.Format("SELECT * FROM t_kpi_dimension WHERE dimension_name= '{0}'", dimension_name);
            return service.FindList(sql).Count > 0;
        }
    }
}
