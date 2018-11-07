using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.AssessmentManage;

namespace KPI.Mapping.AssessmentManage
{
   
    public class AssessmentResultMap : EntityTypeConfiguration<AssessmentResultEntity>
    {
        public AssessmentResultMap()
        {
            this.ToTable("t_kpi_result_detail");
            this.HasKey(t => t.id);
        }
    }
}
