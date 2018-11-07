using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity.TaskManage;

namespace KPI.Mapping.TaskManage
{
    public class TaskDistributionMap : EntityTypeConfiguration<TaskDistributionEntity>
    {
        public TaskDistributionMap()
        {
            this.ToTable("t_kpi_task_distribution");
            this.HasKey(t => t.id);
        }
    }
}
