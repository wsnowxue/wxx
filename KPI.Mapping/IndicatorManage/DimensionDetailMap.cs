using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Mapping.IndicatorManage
{

    public class DimensionDetailMap : EntityTypeConfiguration<DimensionDetailEntity>
    {
        public DimensionDetailMap()
        {
            this.ToTable("t_kpi_dimension_detail");
            this.HasKey(t => t.id);
        }
    }
}
