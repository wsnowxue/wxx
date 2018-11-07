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

namespace KPI.Application.IndicatorManage
{
    public class IndicatorsDefineApp
    {
        private IIndicatorsDefineRepository service = new IndicatorsDefineRepository();

        #region 带起始索引、查询条数的查询
        public List<IndicatorsDefineEntity> GetItemList(string indicator_name, string indicator_define, string count_method, int? statue, Pagination p)
        {
            if (p.rows == 0 || p==null)
            {
                //分页rows=0 获取全部
                return GetIndicatorByStatue(statue);
            }
            var expression = ExtLinq.True<IndicatorsDefineEntity>();
            if (!string.IsNullOrEmpty(indicator_name))
            {
                expression = expression.And(r => r.indicator_name.Contains(indicator_name));
            }
            if (!string.IsNullOrEmpty(indicator_define))
            {
                expression = expression.And(r => r.indicator_define.Contains(indicator_define));
            }
            if (!string.IsNullOrEmpty(count_method))
            {
                expression = expression.And(r => r.indicator_count_method.Contains(count_method));
            }
            if (statue != null) {
                expression = expression.And(r => r.statue== statue);
            }
            return service.FindList(expression, p);
        }
        #endregion

        public List<IndicatorsDefineEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public IndicatorsDefineEntity GetForm(string keyValue)
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
        public void SubmitForm(IndicatorsDefineEntity indicatorsDefineEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                indicatorsDefineEntity.Modify(keyValue);
                service.Update(indicatorsDefineEntity);
            }
            else
            {
                indicatorsDefineEntity.Create();
                service.Insert(indicatorsDefineEntity);
            }
        }

        public IndicatorsDefineEntity GetIndicatorByName(string indicator_name)
        {
            var expression = ExtLinq.True<IndicatorsDefineEntity>();
            if (!string.IsNullOrEmpty(indicator_name))
            {
                expression = expression.And(r => r.indicator_name == indicator_name);
            }
            return service.FindEntity(expression);
        }

        public List<IndicatorsDefineEntity> GetIndicatorByStatue(int? statue)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT * FROM t_indicators_define ");
            if (statue != null) {
                sqlBuilder.AppendFormat("WHERE statue= {0}", statue);
            }
            return service.FindList(sqlBuilder.ToString());
        }

        public bool IsExists(string indicator_name)
        {
            string sql = string.Format("SELECT * FROM t_indicators_define WHERE indicator_name= '{0}'", indicator_name);
            return service.FindList(sql).Count > 0;
        }
    }
}
