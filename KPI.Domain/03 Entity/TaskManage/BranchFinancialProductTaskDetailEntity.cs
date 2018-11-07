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

    public class BranchFinancialProductTaskDetailEntity : IEntityEx<BranchFinancialProductTaskDetailEntity>, ICreationAuditedEx, IModificationAuditedEx
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
        public int traffic { get; set; }
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
        public int traditional_new_car { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int YiRong_loan { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int public_credit { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int car_loaner { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int finanical_leasing { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int second_hand_car { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
