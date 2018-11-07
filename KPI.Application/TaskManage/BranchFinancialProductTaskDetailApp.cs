using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Repository.IndicatorManage;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.IRepository;
using KPI.Code;
using System.Linq.Expressions;
using System;
using KPI.Repository.TempleteManage;
using KPI.Domain.Entity.TempleteManage;
using KPI.Domain.ViewModel;
using KPI.Domain.Entity.TaskManage;
using KPI.Repository.TaskManage;

namespace KPI.Application.TaskManage
{
    public class BranchFinancialProductTaskDetailApp
    {
        private IBranchFinancialProductTaskDetailRepository service = new BranchFinancialProductTaskDetailRepository();

        #region 带起始索引、查询条数的查询
        //public List<BranchFinancialProductTaskDetailEntity> GetItemList(string task_name, string task_statue, string user_id, string pagination)
        //{
        //    Pagination p = null;
        //    if (!string.IsNullOrEmpty(pagination)) {
        //        p = Json.ToObject<Pagination>(pagination);
        //    }

        //    if (p.rows == 0 || p==null)
        //    {
        //        //分页rows=0 获取全部
        //        return GetList();
        //    }
        //    var expression = ExtLinq.True<BranchFinancialProductTaskDetailEntity>();
        //    //if (!string.IsNullOrEmpty(task_name))
        //    //{
        //    //    expression = expression.And(r => r.task_name.Contains(task_name));
        //    //}
        //    if (task_statue != null)
        //    {
        //        expression = expression.And(r => r.statue.Equals(task_statue));
        //    }
        //    if (user_id != null)
        //    {
        //        expression = expression.And(r => r.creator_user_id.Equals(user_id));
        //    }

        //    return service.FindList(expression, p);
        //}
        #endregion


        #region 获取年度任务视图
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="templete_type">方案类型</param>
        /// <returns></returns>
        public List<BranchFinancialProductTaskDetailOverviewModel> GetFinanicalProductsTaskDetailOverview(Pagination pagination, string task_id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
           
                sqlBuilder.Append(@"SELECT ytd.id ,ytd.task_id,ytd.task_object,o.F_FullName AS task_object_name,
                                    ytd.traffic,td.assessment_start_time AS start_date,td.assessment_end_time AS end_date,
                                    ytd.traditional_new_car,ytd.YiRong_loan,ytd.public_credit,ytd.car_loaner,ytd.finanical_leasing,
                                    ytd.second_hand_car ");
                sqlBuilder.Append(@"FROM t_kpi_branch_financial_product_task_detail ytd
                                    LEFT JOIN t_kpi_task_distribution td
                                    ON td.id=ytd.task_id
                                    LEFT JOIN sys_organize o
                                    ON o.F_id=ytd.task_object ");
            sqlBuilder.AppendFormat("WHERE ytd.task_id='{0}'", task_id);

            IEnumerable<BranchFinancialProductTaskDetailOverviewModel> retData = null;
            if (pagination == null || pagination.rows == 0)
            {
                sqlBuilder.Append("ORDER BY ytd.task_object");
                retData = service.BasicQueryListT<BranchFinancialProductTaskDetailOverviewModel>(sqlBuilder.ToString());
            }
            else {
                pagination.sidx = "ytd.task_object";
                retData = service.FindListBySql<BranchFinancialProductTaskDetailOverviewModel>(sqlBuilder.ToString(), pagination);
            }

            return retData == null ? null : retData.ToList();
        }
        #endregion

        public List<BranchFinancialProductTaskDetailEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public BranchFinancialProductTaskDetailEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.id.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.id == keyValue);
            }
        }
        public void SubmitForm(BranchFinancialProductTaskDetailEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
            }
        }
        public BranchFinancialProductTaskDetailEntity SubmitFormEx(BranchFinancialProductTaskDetailEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
            }
            return entity;
        }

        
    }
}
