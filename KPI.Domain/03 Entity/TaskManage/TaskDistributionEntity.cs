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
  
    public class TaskDistributionEntity : IEntityEx<TaskDistributionEntity>, ICreationAuditedEx,IModificationAuditedEx
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public string task_name { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public DateTime? assessment_start_time { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public DateTime? assessment_end_time { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public string task_sponsor { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_distribute_statue { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_type { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public string task_file { get; set; }
        /// <summary>
        /// 状态 0 停用，1 启用        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
