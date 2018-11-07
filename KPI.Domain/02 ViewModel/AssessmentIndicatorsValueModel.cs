using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class AssessmentIndicatorsValueModel
    {
        public string id { get; set; }
        /// <summary>
        /// 考核名称
        /// </summary>
        public string assessment_name { get; set; }
        /// <summary>
        /// 考核id
        /// </summary>
        public string assessment_id { get; set; }
        /// <summary>
        /// 考核人
        /// </summary>
        public string checker { get; set; }
        /// <summary>
        /// 考核对象
        /// </summary>
        public string check_object { get; set; }
        /// <summary>
        /// 指标id
        /// </summary>
        public string indicator_id { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string indicator_name { get; set; }
        /// <summary>
        /// 指标得分
        /// </summary>
        public double? indicator_value { get; set; }
    }
}
