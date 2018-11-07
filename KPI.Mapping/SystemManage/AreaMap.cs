
using KPI.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemManage
{
    public class AreaMap : EntityTypeConfiguration<AreaEntity>
    {
        public AreaMap()
        {
            this.ToTable("sys_area");
            this.HasKey(t => t.F_Id);
        }
    }
}
