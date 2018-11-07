
using System;

namespace KPI.Domain.Entity.AssessmentManage
{
    public class AssessmentResultCountEntity : IEntityEx<AssessmentResultCountEntity>, ICreationAuditedEx, IModificationAuditedEx
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
        /// 模板固定分数
        /// </summary>
        public double? fixed_score { get; set; }
        /// <summary>
        /// 模板不固定分数（可以更改的部分，例如自评）
        /// </summary>
        public double? unfixed_score { get; set; }
        /// <summary>
        /// 考核总得分
        /// </summary>
        public double? total_score { get; set; }
        /// <summary>
        /// 此考评人权重
        /// </summary>
        public double? checker_weight { get; set; }
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
