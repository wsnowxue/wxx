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

    public class BranchCooperateBankTaskDetailEntity : IEntityEx<BranchCooperateBankTaskDetailEntity>, ICreationAuditedEx, IModificationAuditedEx
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
        public int ICBC_NB { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int ICBC_AH { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int CBC { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int ICBC_GD { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public int Bank_WZ { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int other_bank { get; set; }
        /// <summary>
        /// 指标公式id
        /// </summary>
        public int Financial_Lean { get; set; }
        
        /// <summary>
        /// 细项指标
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
