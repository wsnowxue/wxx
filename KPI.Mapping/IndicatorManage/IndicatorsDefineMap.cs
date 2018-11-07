using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Mapping.IndicatorManage
{
   
    public class IndicatorsDefineMap : EntityTypeConfiguration<IndicatorsDefineEntity>
    {
        public IndicatorsDefineMap()
        {
            this.ToTable("t_indicators_define");
            this.HasKey(t => t.id);
        }
    }
}
