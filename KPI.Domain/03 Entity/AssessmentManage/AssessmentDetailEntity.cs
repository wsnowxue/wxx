
using System;

namespace KPI.Domain.Entity.AssessmentManage
{
    public class AssessmentDetailEntity : IEntityEx<AssessmentDetailEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 考核id
        /// </summary>
        public string assessment_id { get; set; }
        /// <summary>
        /// 考核对象
        /// </summary>
        public string check_object { get; set; }
        /// <summary>
        /// 考核人
        /// </summary>
        public string checker { get; set; }
        /// <summary>
        /// 考核人权重
        /// </summary>
        public double? checker_weight { get; set; }
        /// <summary>
        /// 考核人次序
        /// </summary>
        public int check_order { get; set; }
        /// <summary>
        /// 本考核是否完成
        /// </summary>
        public int finished { get; set; }
        /// <summary>
        /// 本考核共需要考核次数
        /// </summary>
        public int check_total_count { get; set; }
        /// <summary>
        /// 0 考核中，1 已完成待归档，2 已归档
        /// </summary>
        public int statue { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string creator_user_id { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? last_modify_time { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time { get; set; }
    }
}
