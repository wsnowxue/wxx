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

    public class YearlyTaskDetailEntity : IEntityEx<YearlyTaskDetailEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public string task_id { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string task_object { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_yearly { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public DateTime? start_date { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public DateTime? end_date { get; set; }
        
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_Jan { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_Feb { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_Mar { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_Apr { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_May { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_Jun { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_Jul { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_Aug { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_Sep { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_Oct { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_Nov { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_Dec { get; set; }
        /// <summary>
        /// 状态 0 停用，1 启用        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
