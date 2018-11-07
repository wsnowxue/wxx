using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.TempleteManage;

namespace KPI.Mapping.TempleteManage
{
    
    public class TempleteCompostionMap : EntityTypeConfiguration<TempleteCompostionEntity>
    {
        public TempleteCompostionMap()
        {
            this.ToTable("t_kpi_templete_compostion");
            this.HasKey(t => t.id);
        }
    }
}
