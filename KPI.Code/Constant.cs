using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Code
{
    public class Constant
    {
        #region 任务类型 枚举
        public enum TaskType
        {
            待考评 = 1,
            待审核,
            待自评,
            待归档,
            待分发

        }
        #endregion

        public static string[] YearlyTaskTempleteCompanyListColumn = { "分公司名称", "全年任务指标", "1月份月度分解数（万）", "2月份月度分解数（万）", "3月份月度分解数（万）", "4月份月度分解数（万）", "5月份月度分解数（万）", "6月份月度分解数（万）", "7月份月度分解数（万）", "8月份月度分解数（万）", "9月份月度分解数（万）", "10月份月度分解数（万）", "11月份月度分解数（万）", "12月份月度分解数（万）" };
        public static string[] YearlyTaskTempletePersonListColumn = { "姓名", "全年任务指标", "1月份月度分解数（万）", "2月份月度分解数（万）", "3月份月度分解数（万）", "4月份月度分解数（万）", "5月份月度分解数（万）", "6月份月度分解数（万）", "7月份月度分解数（万）", "8月份月度分解数（万）", "9月份月度分解数（万）", "10月份月度分解数（万）", "11月份月度分解数（万）", "12月份月度分解数（万）", "手机号" };
        public static string[] FinancialProductTaskTempleteListColumn = { "分公司名称", "业务量（万）", "传统新车（万）", "易融贷（万）", "公牌私贷（万）", "车主贷（万）", "融资租赁（万）", "二手车（万）" };
        public static string[] CooperationBankTaskTempleteListColumn = { "分公司名称", "业务量（万）", "宁波工行（万）", "安徽工行（万）", "建设银行（万）", "广东工行（万）", "温州银行（万）", "其他银行（万）", "融资租赁（万）" };
        public static string[] CustomerManagerTempleteListColumn = { "姓名", "员工编号", "手机号码", "身份证号码", "职务", "性别", "所属部门" ,"所属分公司"};

    }
}
