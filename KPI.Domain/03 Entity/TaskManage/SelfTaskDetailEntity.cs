using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.TaskManage
{
    /// <summary>
    /// 绩效考核维度细项
    /// </summary>

    public class SelfTaskDetailEntity : IEntityEx<SelfTaskDetailEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_type { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public string person { get; set; }
        /// <summary>
        /// 任务对象
        /// </summary>
        public string task_object { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public int task_statue { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string attach { get; set; }
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
