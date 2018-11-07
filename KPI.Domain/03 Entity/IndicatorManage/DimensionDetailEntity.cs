using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.IndicatorManage
{
    /// <summary>
    /// 绩效考核维度细项
    /// </summary>
  
    public class DimensionDetailEntity : IEntityEx<DimensionDetailEntity>, ICreationAuditedEx,IModificationAuditedEx
    {

        public string id { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public string detail_name { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public string formule { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public string method_id { get; set; }
        /// <summary>
        /// 状态 0 停用，1 启用        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
