using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.IndicatorManage;

namespace KPI.Mapping.IndicatorManage
{
   
    public class CheckMethodMap : EntityTypeConfiguration<CheckMethodEntity>
    {
        public CheckMethodMap()
        {
            this.ToTable("t_check_method");
            this.HasKey(t => t.id);
        }
    }
}
