/*******************************************************************************
 * Copyright © 2016 KPI.Framework 版权所有
 * Author: KPI
 * Description: KPI快速开发平台
 * Website：http://www.KPI.cn
*********************************************************************************/
using KPI.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace KPI.Mapping.SystemSecurity
{
    public class FilterIPMap : EntityTypeConfiguration<FilterIPEntity>
    {
        public FilterIPMap()
        {
            this.ToTable("sys_filterip");
            this.HasKey(t => t.F_Id);
        }
    }
}
