using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.IndicatorManage
{
    /// <summary>
    /// 绩效考核维度
    /// </summary>
  
    public class DimensionEntity : IEntityEx<DimensionEntity>, ICreationAuditedEx,IModificationAuditedEx
    {

        public string id { get; set; }
        /// <summary>
        /// 考核维度名称
        /// </summary>
        public string dimension_name { get; set; }
        /// <summary>
        /// 状态 0 停用，1 启用        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
