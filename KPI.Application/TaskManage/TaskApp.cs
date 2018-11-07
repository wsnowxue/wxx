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
using KPI.Application.SystemManage;
using KPI.Domain.Entity.SystemManage;

namespace KPI.Application.TaskManage
{
    public class TaskApp
    {
        private ITaskDistributionRepository service = new TaskDistributionRepository();
        private RoleApp roleApp = new RoleApp();

        #region 带起始索引、查询条数的查询
        public List<TaskDistributionEntity> GetItemList(string task_name, int? task_statue, int? task_type, string user_id, Pagination pagination)
        {
            var expression = ExtLinq.True<TaskDistributionEntity>();
            expression = expression.And(r => r.statue==0);
            if (!string.IsNullOrEmpty(task_name))
            {
                expression = expression.And(r => r.task_name.Contains(task_name));
            }
            if (task_statue!= null)
            {
                expression = expression.And(r => r.task_distribute_statue==task_statue);
            }
            if (task_type != null)
            {
                expression = expression.And(r => r.task_type == task_type);
            }
            if (!string.IsNullOrEmpty(user_id))
            {
                expression = expression.And(r => r.creator_user_id.Equals(user_id));
            }
            
            return service.FindList(expression, pagination);
        }

        #endregion
        #region 获取系统默认考核方案
        public List<TempleteEntity> GetDefaultItemList(int? templete_type)
        {
            string sql = "";
            if (templete_type != null)
            {
                sql = "select * from t_kpi_templete where templete_type= "+ templete_type;
            }
            //return service.FindList(sql);
            return null;
        }
        #endregion

        #region 获取系统默认考核方案
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="templete_type">方案类型</param>
        /// <returns></returns>
        public List<TaskOverviewModel> GetTaskListOverview( Pagination pagination, string task_name, int? task_statue, int? task_type, string user_id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT  td.id,td.task_name,td.assessment_start_time ,
                                td.assessment_end_time ,u.F_RealName AS task_sponsor_name,td.creator_user_id,
                                uu.F_RealName AS creator_user_name,
                                td.task_distribute_statue AS task_distribute_statue,td.task_type,td.create_time ");
            sqlBuilder.Append(@"FROM t_kpi_task_distribution td
                                LEFT JOIN  sys_user u
                                ON td.task_sponsor=u.F_Id 
                                LEFT JOIN sys_user uu
                                ON td.creator_user_id=uu.F_Id ");
            sqlBuilder.AppendFormat("WHERE td.statue=0 ");
           
            if (!string.IsNullOrEmpty(task_name))
            {
                sqlBuilder.AppendFormat("AND td.task_name like '%{0}%' ", task_name);
            }
            
            if (task_statue!=null)
            {
                sqlBuilder.AppendFormat("AND td.task_distribute_statue={0} ", task_statue);
            }
            if (task_type != null)
            {
                sqlBuilder.AppendFormat("AND td.task_type={0} ", task_type);
                if (task_type == 1) {
                    //若个人方案
                    sqlBuilder.AppendFormat("AND uu.F_OrganizeId='{0}' ", OperatorProvider.Provider.GetCurrent().CompanyId);
                }

            }

            //客户经理权限：个人任务管理里只能看到已经分发给自己的任务
            if (OperatorProvider.Provider.GetCurrent() == null) return null;
            RoleEntity roleEntity = roleApp.GetForm(OperatorProvider.Provider.GetCurrent().RoleId);
            if (roleEntity == null)
            {
                return null;
            }
            if (roleEntity.F_FullName.Equals("客户经理") || roleEntity.F_FullName.Equals("经理室"))
            {
                StringBuilder sqlBuilder2 = new StringBuilder();
                sqlBuilder2.AppendFormat(@"SELECT a.* FROM ({0}) a 
                                                INNER JOIN t_self_task_detail st
                                                ON st.task_object=a.id
                                                AND st.task_statue=2
                                                AND st.person='{1}' ", sqlBuilder, OperatorProvider.Provider.GetCurrent().UserId);
                sqlBuilder = sqlBuilder2;
            }

            IEnumerable<TaskOverviewModel> retData = null;
            if (pagination == null || pagination.rows == 0)
                retData = service.BasicQueryListT<TaskOverviewModel>(sqlBuilder.ToString());
            else
                retData = service.FindListBySql<TaskOverviewModel>(sqlBuilder.ToString(), pagination);
            return retData == null ? null : retData.ToList();
        }
        #endregion

        #region 获取系统默认考核方案
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="id">分页</param>
        /// <param name="templete_type">方案类型</param>
        /// <returns></returns>
        public List<TempleteOverviewModel> GetTempleteOverview(string id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT  t.id AS id,t.id AS templete_id,c.statue AS templete_check_statue,c.statue AS statue,t.templete_type,cm.dimension_id,cm.detail_id,dd.formule as indicator_formula_name,cm.allow_beyond_limit,
                                c.check_suggest as back_reason, t.creator_user_id, t.last_modify_time, t.create_time ");
            sqlBuilder.Append(@" FROM 
                                    t_kpi_templete t
                                 LEFT JOIN t_kpi_templete_check c
                                ON t.id = c.templete_id
                                LEFT JOIN t_kpi_templete_compostion cm
                                ON t.id = cm.templete_id
                                LEFT JOIN t_kpi_dimension_detail dd
                                ON dd.id = cm.dimension_id ");
            sqlBuilder.AppendFormat("WHERE t.id='{0}' ", id);

            IEnumerable<TempleteOverviewModel> retData = null;
            retData = service.BasicQueryListT<TempleteOverviewModel>(sqlBuilder.ToString());
            return retData.ToList();
        }
        #endregion

        

        public List<TaskDistributionEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public TaskDistributionEntity GetForm(string keyValue)
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
        public void SubmitForm(TaskDistributionEntity TempleteEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                TempleteEntity.Modify(keyValue);
                service.Update(TempleteEntity);
            }
            else
            {
                TempleteEntity.Create();
                service.Insert(TempleteEntity);
            }
        }
        public TaskDistributionEntity SubmitFormEx(TaskDistributionEntity templeteEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                templeteEntity.Modify(keyValue);
                service.Update(templeteEntity);
            }
            else
            {
                templeteEntity.Create();
                templeteEntity.task_sponsor = templeteEntity.creator_user_id;//任务分发人默认为：任务创建者
                service.Insert(templeteEntity);
            }
            return templeteEntity;
        }

        public TaskDistributionEntity GetTaskById(string task_id)
        {
            var expression = ExtLinq.True<TaskDistributionEntity>();
            if (!string.IsNullOrEmpty(task_id))
            {
                expression = expression.And(r => r.id == task_id);
            }
            return service.FindEntity(expression);
        }

        public bool IsExists(string task_name)
        {
            string sql = string.Format("SELECT * FROM t_kpi_task_distribution WHERE task_name= '{0}'", task_name);
            return service.FindList(sql).Count > 0;
        }
    }
}
