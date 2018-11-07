
using KPI.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemManage
{
    public class UserLogOnMap : EntityTypeConfiguration<UserLogOnEntity>
    {
        public UserLogOnMap()
        {
            this.ToTable("sys_userlogon");
            this.HasKey(t => t.F_Id);
        }
    }
}
