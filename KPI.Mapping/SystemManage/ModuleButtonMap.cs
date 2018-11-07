
using KPI.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemManage
{
    public class ModuleButtonMap : EntityTypeConfiguration<ModuleButtonEntity>
    {
        public ModuleButtonMap()
        {
            this.ToTable("sys_modulebutton");
            this.HasKey(t => t.F_Id);
        }
    }
}
