using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Data;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Domain.IRepository
{
    public interface IDimensionRepository : IRepositoryBase<DimensionEntity>
    {
       // List<DimensionEntity> GetItemList(string dimension_name, int start_index, int total);
    }
    
}
