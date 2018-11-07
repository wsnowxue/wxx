
using System;

namespace KPI.Domain.Entity.AssessmentManage
{
    public class AssessmentResultEntity : IEntityEx<AssessmentResultEntity>, ICreationAuditedEx, IModificationAuditedEx
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
        /// 考评人
        /// </summary>
        public string checker { get; set; }
        /// <summary>
        /// 细项指标
        /// </summary>
        public string indicator_id { get; set; }
        /// <summary>
        /// 细项指标完成值
        /// </summary>
        public double? indicator_value { get; set; }
        /// <summary>
        /// 0 未删除，1 已删除
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
