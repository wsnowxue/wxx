using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using KPI.Domain.Entity.TaskManage;

namespace KPI.Mapping.TaskManage
{
    public class SelfTaskDetailMap : EntityTypeConfiguration<SelfTaskDetailEntity>
    {
        public SelfTaskDetailMap()
        {
            this.ToTable("t_self_task_detail");
            this.HasKey(t => t.id);
        }
    }
}
