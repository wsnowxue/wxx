
using KPI.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemManage
{
    public class RoleAuthorizeMap : EntityTypeConfiguration<RoleAuthorizeEntity>
    {
        public RoleAuthorizeMap()
        {
            this.ToTable("sys_roleauthorize");
            this.HasKey(t => t.F_Id);
        }
    }
}
