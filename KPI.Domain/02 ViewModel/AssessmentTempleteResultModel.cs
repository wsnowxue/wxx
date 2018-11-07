using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class AssessmentTempleteResultModel
    {
        /// <summary>
        /// 考核id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 考核名称
        /// </summary>
        public string assessment_name { get; set; }
        /// <summary>
        /// 考核模板
        /// </summary>
        public string templete_id { get; set; }
        /// <summary>
        /// 考核对象
        /// </summary>
        public string check_object { get; set; }
        /// <summary>
        /// 维度id
        /// </summary>
        public string dimension_id { get; set; }
        /// <summary>
        /// 维度名称
        /// </summary>
        public string dimension_name { get; set; }
        /// <summary>
        /// 细项id
        /// </summary>
        public string dimension_detail_id { get; set; }
        /// <summary>
        /// 细项名称
        /// </summary>
        public string dimension_detail_name { get; set; }
        /// <summary>
        /// 基础分值
        /// </summary>
        public double? base_score { get; set; }
        /// <summary>
        /// 基础分标准值
        /// </summary>
        public double? base_standard_score { get; set; }
        /// <summary>
        /// 实际完成
        /// </summary>
        public double? finish { get; set;}
        /// <summary>
        /// 指标公式
        /// </summary>
        public string formule { get; set; }
        /// <summary>
        /// 考核办法id
        /// </summary>
        public string method_id { get; set; }
        /// <summary>
        /// 考核办法
        /// </summary>
        public string check_method { get; set; }
        /// <summary>
        /// 考核办法处理
        /// </summary>
        public string check_method_proc { get; set; }
        /// <summary>
        /// 考评得分
        /// </summary>
        public double? assessment_result { get; set; }
        /// <summary>
        /// 考评总分数
        /// </summary>
        public double? total_result { get; set; }
    }
}
