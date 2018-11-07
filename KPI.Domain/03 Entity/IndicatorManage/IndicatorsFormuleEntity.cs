using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.IndicatorManage
{

    public class IndicatorsFormuleEntity : IEntityEx<IndicatorsFormuleEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        public string id { get; set; }
        /// <summary>
        /// 指标计算方式名称
        /// </summary>
        public string formule_name { get; set; }
        /// <summary>
        /// 指标公式定义
        /// </summary>
        public string formule_CountMethod { get; set; }
        /// <summary>
        /// 指标公式计算方式
        /// </summary>
        public string formule { get; set; }
        /// <summary>
        /// 启用标志 0停用 1启用
        /// </summary>
        public string statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }
    }
}
