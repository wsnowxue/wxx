using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Domain.Entity;
using KPI.Domain.Entity.TaskManage;

namespace KPI.Mapping.TaskManage
{
    public class BranchFinancialProductTaskDetailMap : EntityTypeConfiguration<BranchFinancialProductTaskDetailEntity>
    {
        public BranchFinancialProductTaskDetailMap()
        {
            this.ToTable("t_kpi_branch_financial_product_task_detail");
            this.HasKey(t => t.id);
        }
    }
}
