using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class TempleteOverviewModel
    {
        /// <summary>
        /// 方案id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 方案id
        /// </summary>
        public string templete_id { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string templete_name { get; set; }
        /// <summary>
        /// 方案审核状态 0 待审核，1 通过，2退回
        /// </summary>
        public int? templete_check_statue { get; set; }
        ///// <summary>
        ///// 启用状态 0 停用，1 启用
        ///// </summary>
        public int statue { get; set; }
        /// <summary>
        /// 方案类型 集团考核分公司模板，1 分公司考核个人模板
        /// </summary>
        public int templete_type { get; set; }
        /// <summary>
        /// 维度id
        /// </summary>
        public string dimension_id { get; set; }
        /// <summary>
        /// 维度名称
        /// </summary>
        public string dimension_name { get; set; }
        /// <summary>
        /// 细则id
        /// </summary>
        public string detail_id { get; set; }
        /// <summary>
        /// 细项名称
        /// </summary>
        public string detail_name { get; set; }
        /// <summary>
        /// 指标公式名称
        /// </summary>
        public string indicator_formula_name { get; set; }
        /// <summary>
        /// 考核办法名字
        /// </summary>
        public string check_method_name { get; set; }
        /// <summary>
        /// 考核办法
        /// </summary>
        public string check_method_define { get; set; }
        /// <summary>
        /// 基础分值
        /// </summary>
        public float base_score { get; set; }
        /// <summary>
        /// 允许评分超限 0 不允许，1允许
        /// </summary>
        public int allow_beyond_limit { get; set; }
        /// <summary>
        /// 0 停用，1启用
        /// </summary>
        public string back_reason { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string creator_user_id { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string creator_user_name { get; set; }
        /// <summary>
        /// 创建人职位名称
        /// </summary>
        public string creator_user_duty { get; set; }
        /// <summary>
        /// 创建人组织名称
        /// </summary>
        public string creator_user_org { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string checker_id { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string checker_name { get; set; }
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
