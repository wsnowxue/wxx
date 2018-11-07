using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Mapping.IndicatorManage
{
   
    public class IndicatorsCountMethodMap : EntityTypeConfiguration<IndicatorsCountMethodEntity>
    {
        public IndicatorsCountMethodMap()
        {
            this.ToTable("t_indicators_count_method");
            this.HasKey(t => t.id);
        }
    }
}
