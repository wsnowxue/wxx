using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.TempleteManage
{
    public class TempleteEntity : IEntityEx<TempleteEntity>, ICreationAuditedEx, IModificationAuditedEx
    {

        public string id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string templete_name { get; set; }
        /// <summary>
        /// 模板审核状态
        /// </summary>
        public int templete_check_statue { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public int templete_type { get; set; }
        /// <summary>
        /// 模板启动标志 状态 0 停用，1 启用        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
