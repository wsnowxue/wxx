using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.IndicatorManage
{
  
    public class IndicatorsDefineEntity : IEntityEx<IndicatorsDefineEntity>, ICreationAuditedEx,IModificationAuditedEx
    {

        public string id { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string indicator_name { get; set; }
        /// <summary>
        /// 指标定义
        /// </summary>
        public string indicator_define { get; set; }
        /// <summary>
        /// 指标统计方式
        /// </summary>
        public string indicator_count_method { get; set; }
        /// <summary>
        /// 指标编码
        /// </summary>
        public string indicator_code { get; set; }
        /// <summary>
        /// 状态 0 停用，1 启用        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
