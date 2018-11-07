
using System;

namespace KPI.Domain.Entity.AssessmentManage
{
    public class AssessmentEntity : IEntityEx<AssessmentEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 考核名称
        /// </summary>
        public string assessment_name { get; set; }
        /// <summary>
        /// 考核模板id
        /// </summary>
        public string templete_id { get; set; }
        /// <summary>
        /// 考核开始时间
        /// </summary>
        public DateTime? start_time { get; set; }
        /// <summary>
        /// 考核结束时间
        /// </summary>
        public DateTime? end_time { get; set; }
        /// <summary>
        /// 考核发起人
        /// </summary>
        public string assessment_sponsor { get; set; }
        /// <summary>
        /// 考核数量
        /// </summary>
        public int assessment_count { get; set; }
        /// <summary>
        /// 考核状态 0 考核中，1 待归档，2 已归档
        /// </summary>
        public int assessment_statue { get; set; }
        /// <summary>
        /// 考核类型 0 集团考核分公司，1 分公司考核个人
        /// </summary>
        public int assessment_type { get; set; }
        /// <summary>
        /// 是否需要自评 0 不需要，1 需要
        /// </summary>
        public int need_self_check { get; set; }
        /// <summary>
        /// 归档人
        /// </summary>
        public string filing_people { get; set; }
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
