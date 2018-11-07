using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Repository.IndicatorManage;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.IRepository;
using KPI.Code;

namespace KPI.Application.IndicatorManage
{
    public class IndicatorsCountMethodApp
    {
        private IIndicatorsCountMethodRepository service = new IndicatorsCountMethodRepository();

        #region 带起始索引、查询条数的查询
        //public List<IndicatorsCountMethodEntity> GetItemList(int start_index, int total)
        //{
        //    return service.GetItemList(start_index, total);
        //}
        #endregion

        #region 带起始索引、查询条数的查询
        public List<IndicatorsCountMethodEntity> GetItemList(string pagination)
        {
            Pagination p = Json.ToObject<Pagination>(pagination);
            if (p.rows == 0)
            {
                //分页rows=0 获取全部
                return GetList();
            }
            return service.FindList(p);
        }
        #endregion


        public List<IndicatorsCountMethodEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public IndicatorsCountMethodEntity GetForm(string keyValue)
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
        public void SubmitForm(IndicatorsCountMethodEntity indicatorsCountMethodEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                indicatorsCountMethodEntity.Modify(keyValue);
                service.Update(indicatorsCountMethodEntity);
            }
            else
            {
                indicatorsCountMethodEntity.Create();
                service.Insert(indicatorsCountMethodEntity);
            }
        }
    }
}
