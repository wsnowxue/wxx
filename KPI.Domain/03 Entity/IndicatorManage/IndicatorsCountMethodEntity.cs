using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.IndicatorManage
{

    public class IndicatorsCountMethodEntity : IEntityEx<IndicatorsCountMethodEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        public string id { get; set; }
        /// <summary>
        /// 统计方式名称
        /// </summary>
        public string count_method_name { get; set; }
        /// <summary>
        /// 处理类
        /// </summary>
        public string proc_class_name { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
