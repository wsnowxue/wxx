using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class DimensionDetailOverviewModel
    {
        public string id { get; set; }
        /// <summary>
        /// 细项名称
        /// </summary>
        public string detail_name { get; set; }
        /// <summary>
        /// 公式
        /// </summary>
        public string formule { get; set; }
        /// <summary>
        /// 考核方法的定义
        /// </summary>
        public string check_method_define { get; set; }
        /// <summary>
        /// 考核方法名称
        /// </summary>
        public string check_method_name { get; set; }
        /// <summary>
        /// 状态 0 停用，1 启用
        /// </summary>
        public int statue { get; set; }

        public DateTime? create_time { get; set; }
    }
}
