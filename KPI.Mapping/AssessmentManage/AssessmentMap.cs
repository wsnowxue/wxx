using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.AssessmentManage;

namespace KPI.Mapping.AssessmentManage
{
   
    public class AssessmentMap : EntityTypeConfiguration<AssessmentEntity>
    {
        public AssessmentMap()
        {
            this.ToTable("t_kpi_launch");
            this.HasKey(t => t.id);
        }
    }
}
