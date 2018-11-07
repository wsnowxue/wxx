using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.IndicatorManage
{
    public class CheckMethodEntity : IEntityEx<IndicatorsCountMethodEntity>, ICreationAuditedEx, IModificationAuditedEx
    {
        public string id { get; set; }
        /// <summary>
        /// 考核方法名称
        /// </summary>
        public string check_method_name { get; set; }
        /// <summary>
        /// 考核方法定义
        /// </summary>
        public string check_method_define { get; set; }
        /// <summary>
        /// 考核方法状态 0 停用，1 启用
        /// </summary>
        public int check_method_statue { get; set; }
        /// <summary>
        /// 处理
        /// </summary>
        public string check_method_proc { get; set; }
        /// <summary>
        /// 删除状态 0 已删除，1 未删除
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }
    }
}
