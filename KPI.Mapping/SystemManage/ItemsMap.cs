
using KPI.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemManage
{
    public class ItemsMap : EntityTypeConfiguration<ItemsEntity>
    {
        public ItemsMap()
        {
            this.ToTable("sys_items");
            this.HasKey(t => t.F_Id);
        }
    }
}
