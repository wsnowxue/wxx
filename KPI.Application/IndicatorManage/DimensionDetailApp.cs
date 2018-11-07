using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Repository.IndicatorManage;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.IRepository;
using System;
using KPI.Code;
using System.Linq.Expressions;
using KPI.Domain.ViewModel;

namespace KPI.Application.IndicatorManage
{
    public class DimensionDetailApp
    {
        private IDimensionDetailRepository service = new DimensionDetailRepository();

        #region 带起始索引、查询条数的查询
        /// <summary>
        /// 获取细项
        /// </summary>
        /// <param name="detail_name">细项名称</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<DimensionDetailEntity> GetItemList(string detail_name, Pagination pagination)
        {

            if (pagination.rows == 0 || pagination == null)
            {
                //分页rows=0 获取全部
                return GetList();
            }

            Expression<Func<DimensionDetailEntity, bool>> filter = r => true;
            if (!string.IsNullOrEmpty(detail_name))
            {
                filter = filter.And(r => r.detail_name.Contains(detail_name));
            }
            return service.FindList(filter, pagination);
        }
        #endregion
        public List<DimensionDetailOverviewModel> GetDimensionDetailOverview(Pagination pagination, string detail_name, int? statue)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT dd.id,dd.detail_name,dd.formule,cm.check_method_name,cm.check_method_define,dd.statue,dd.create_time ");
            sqlBuilder.Append(@"FROM t_kpi_dimension_detail dd
                                LEFT JOIN t_check_method cm
                                ON dd.method_id=cm.id ");
            sqlBuilder.AppendFormat(" where dd.detail_name like '%{0}%' ", detail_name);
            if (statue != null)
            {
                sqlBuilder.AppendFormat(" and dd.statue ={0} ", statue);
            }

            IEnumerable<DimensionDetailOverviewModel> retData = null;
            if (pagination == null || pagination.rows == 0)
                retData = service.BasicQueryListT<DimensionDetailOverviewModel>(sqlBuilder.ToString());
            else
                retData = service.FindListBySql<DimensionDetailOverviewModel>(sqlBuilder.ToString(), pagination);
            return retData == null ? null : retData.ToList();
        }


        public List<DimensionDetailEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public DimensionDetailEntity GetForm(string keyValue)
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
        public void SubmitForm(DimensionDetailEntity dimensionDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                dimensionDetailEntity.Modify(keyValue);
                service.Update(dimensionDetailEntity);
            }
            else
            {
                dimensionDetailEntity.Create();
                service.Insert(dimensionDetailEntity);
            }
        }

        public List<DimensionDetailEntity> GetTempleteDimensionDetail(string templete_id)
        {
            string sql = string.Format("select b.* from t_kpi_templete_compostion a left join t_kpi_dimension_detail b on a.detail_id = b.id where a.templete_id = '{0}'", templete_id);
            return service.FindList(sql);
        }

        public bool IsExists(string detail_name)
        {
            string sql = string.Format("SELECT * FROM t_kpi_dimension_detail WHERE detail_name= '{0}'", detail_name);
            return service.FindList(sql).Count > 0;
        }

        //public List<DimensionDetailOverviewModel> GetDimensionDetailByStatue(int? statue)
        //{
        //    StringBuilder sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append(@"SELECT * FROM t_kpi_dimension_detail ");
        //    if (statue != null)
        //    {
        //        sqlBuilder.AppendFormat("WHERE statue= {0}", statue);
        //    }
        //    return service.FindList(sqlBuilder.ToString());
        //}
    }
}
