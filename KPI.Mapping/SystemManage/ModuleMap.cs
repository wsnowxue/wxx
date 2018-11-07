
using KPI.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemManage
{
    public class ModuleMap : EntityTypeConfiguration<ModuleEntity>
    {
        public ModuleMap()
        {
            this.ToTable("sys_module");
            this.HasKey(t => t.F_Id);
        }
    }
}
