using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using KPI.Domain.Entity.TempleteManage;

namespace KPI.Mapping.TempleteManage
{
    public class TempleteCheckMap : EntityTypeConfiguration<TempleteCheckEntity>
    {
        public TempleteCheckMap()
        {
            this.ToTable("t_kpi_templete_check");
            this.HasKey(t => t.id);
        }
    }
}
