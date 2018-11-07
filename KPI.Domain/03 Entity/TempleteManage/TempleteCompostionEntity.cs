using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.TempleteManage
{
    public class TempleteCompostionEntity : IEntityEx<TempleteCompostionEntity>, ICreationAuditedEx, IModificationAuditedEx
    {

        public string id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string templete_id { get; set; }
        /// <summary>
        /// 模板审核状态
        /// </summary>
        public string dimension_id { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public string detail_id { get; set; }
        public float base_score { get; set; }
        public int allow_beyond_limit { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
