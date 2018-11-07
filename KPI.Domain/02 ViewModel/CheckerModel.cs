using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Domain.ViewModel
{
    public class CheckerModel
    {
        public string id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string role_name { get; set; }
        /// <summary>
        /// 机构
        /// </summary>
        public string organize_name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department_name { get; set; }
    }
}
