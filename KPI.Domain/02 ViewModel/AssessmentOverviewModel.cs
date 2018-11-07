using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class AssessmentOverviewModel
    {
        public string id { get; set; }
        /// <summary>
        /// 考核名称
        /// </summary>
        public string assessment_name { get; set; }
        /// <summary>
        /// 考核模板名称
        /// </summary>
        public string templete_name { get; set; }
        /// <summary>
        /// 考核开始时间
        /// </summary>
        public DateTime? start_time { get; set; }
        /// <summary>
        /// 考核结束时间
        /// </summary>
        public DateTime? end_time { get; set; }
        /// <summary>
        /// 考核发起人id
        /// </summary>
        public string assessment_sponsor_id { get; set; }
        /// <summary>
        /// 归档人
        /// </summary>
        public string filing_people { get; set; }
        /// <summary>
        /// 考核发起人名字
        /// </summary>
        public string assessment_sponsor_name { get; set; }
        /// <summary>
        /// 考核数量
        /// </summary>
        public int assessment_count { get; set; }
        /// <summary>
        /// 考核状态 0 考核中，1 待归档，2 已归档
        /// </summary>
        public int assessment_statue { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time { get; set; }
    }
}
