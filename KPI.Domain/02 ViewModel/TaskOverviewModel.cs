using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class TaskOverviewModel
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
        public string task_sponsor_name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string creator_user_id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string creator_user_name { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int task_distribute_statue { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int task_type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time { get; set; }

        
    }
}
