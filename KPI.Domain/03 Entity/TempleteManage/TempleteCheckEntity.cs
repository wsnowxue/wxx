using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.Entity.TempleteManage
{
    public class TempleteCheckEntity : IEntityEx<TempleteCheckEntity>, ICreationAuditedEx, IModificationAuditedEx
    {

        public string id { get; set; }
        /// <summary>
        /// 待审核模板
        /// </summary>
        public string templete_id { get; set; }
        /// <summary>
        /// 审核发起人
        /// </summary>
        public string check_sponser { get; set; }
        /// <summary>
        /// 审核方
        /// </summary>
        public string checker { get; set; }
        /// <summary>
        /// 审核结果   0 不通过，1 通过 
        /// </summary>
        public int check_result { get; set; }
        /// <summary>
        /// 审核意见如果未通过则必须填写未通过的原因
        /// </summary>
        public string check_suggest { get; set; }
        /// <summary>
        /// 删除标志 0 未删除，1 已删除        
        /// </summary>
        public int statue { get; set; }
        public string creator_user_id { get; set; }
        public DateTime? last_modify_time { get; set; }
        public DateTime? create_time { get; set; }

    }
}
