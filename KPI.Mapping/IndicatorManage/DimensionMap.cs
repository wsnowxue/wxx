using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Mapping.IndicatorManage
{
   
    public class DimensionMap : EntityTypeConfiguration<DimensionEntity>
    {
        public DimensionMap()
        {
            this.ToTable("t_kpi_dimension");
            this.HasKey(t => t.id);
        }
    }
}
