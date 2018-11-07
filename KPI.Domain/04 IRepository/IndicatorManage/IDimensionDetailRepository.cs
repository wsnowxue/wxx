using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Data;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Domain.IRepository
{
    public interface IDimensionDetailRepository : IRepositoryBase<DimensionDetailEntity>
    {
       // List<DimensionDetailEntity> GetItemList(string indicator_name, string indicator_CountMethod, string count_method, int start_index, int total);
    }
    
}
