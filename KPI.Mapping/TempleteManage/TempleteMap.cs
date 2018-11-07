using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity;
using KPI.Domain.Entity.TempleteManage;

namespace KPI.Mapping.TempleteManage
{
    public class TempleteMap : EntityTypeConfiguration<TempleteEntity>
    {
        public TempleteMap()
        {
            this.ToTable("t_kpi_templete");
            this.HasKey(t => t.id);
        }
    }
}
