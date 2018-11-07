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

namespace KPI.Application.TempleteManage
{
    public class TempleteApp
    {
        private ITempleteRepository service = new TempleteRepository();

        #region 带起始索引、查询条数的查询
        public List<TempleteEntity> GetItemList(string templete_name, int? templete_check_statue, int? templete_type, int? statue, Pagination pagination)
        {
            var expression = ExtLinq.True<TempleteEntity>();
            if (!string.IsNullOrEmpty(templete_name))
            {
                expression = expression.And(r => r.templete_name.Contains(templete_name));
            }
            if (templete_check_statue != null)
            {
                expression = expression.And(r => r.templete_check_statue == templete_check_statue);
            }
            if (templete_type != null)
            {
                expression = expression.And(r => r.templete_type == templete_type);
            }
            if (statue != null)
            {
                expression = expression.And(r => r.statue == statue);
            }
            return service.FindList(expression, pagination);
        }

        #endregion
        #region 获取空白考核方案
        public List<TempleteEntity> GetList(int? templete_type)
        {
            string sql = "";
            if (templete_type != null)
            {
                sql = "select * from t_kpi_templete where templete_type= " + templete_type+ " order by create_time desc";
            }
            return service.FindList(sql);
        }


        public List<TempleteOverviewModel> GetBlankTemplete()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT c.id AS id,t.id AS templete_id,t.templete_name, t.templete_check_statue,t.statue AS statue,t.templete_type,cm.dimension_id,d.dimension_name,cm.detail_id,dd.detail_name,dd.formule AS indicator_formula_name,cm.allow_beyond_limit,
                                c.check_suggest AS back_reason, t.creator_user_id,u.F_RealName AS creator_user_name,r.F_FullName AS creator_user_duty, o.F_FullName AS creator_user_org, t.last_modify_time, t.create_time ");
            sqlBuilder.Append(@" FROM t_kpi_templete t
                                 LEFT JOIN t_kpi_templete_check c
                                ON t.id = c.templete_id
                                LEFT JOIN t_kpi_templete_compostion cm
                                ON t.id = cm.templete_id
                                LEFT JOIN t_kpi_dimension_detail dd
                                ON dd.id = cm.detail_id 
                                LEFT JOIN t_kpi_dimension d
                                ON d.id=cm.dimension_id 
                                LEFT JOIN sys_user u
                                ON u.F_Id=t.creator_user_id
                                LEFT JOIN sys_role r
                                ON r.F_id=u.F_DutyId
                                LEFT JOIN sys_organize o
                                ON o.F_Id=u.F_DepartmentId ");
            IEnumerable<TempleteOverviewModel> retData = null;
            retData = service.BasicQueryListT<TempleteOverviewModel>(sqlBuilder.ToString());
            return retData.ToList();
        }
        #endregion

        #region 获取自己创建的考核方案
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="templete_type">方案类型</param>
        /// <returns></returns>
        public List<TempleteOverviewModel> GetTempleteOverview(int? templete_type, string templete_name, int? statue, int? templete_check_statue, string creator_user_name, Pagination pagination)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT t.id AS templete_id,t.templete_name, t.templete_check_statue,t.statue ,t.templete_type,
                                 t.creator_user_id, u.F_RealName AS creator_user_name,r.F_FullName AS creator_user_duty, o.F_FullName AS creator_user_org, t.last_modify_time, t.create_time ");
            sqlBuilder.Append(@"FROM t_kpi_templete t
                                LEFT JOIN sys_user u
                                ON u.F_Id=t.creator_user_id
                                LEFT JOIN sys_role r
                                ON r.F_id=u.F_DutyId
                                LEFT JOIN sys_organize o
                                ON o.F_Id=u.F_DepartmentId ");
            sqlBuilder.AppendFormat(" where 1=1 ");
            if (!string.IsNullOrEmpty(templete_name))
            {
                sqlBuilder.AppendFormat(" and t.templete_name like '%{0}%' ", templete_name);
            }
            if (templete_check_statue != null)
            {
                sqlBuilder.AppendFormat(" and t.templete_check_statue={0} ", templete_check_statue);
            }
            if (templete_type != null)
            {
                sqlBuilder.AppendFormat(" and t.templete_type={0} ", templete_type);
                if (templete_type == 1) {
                    sqlBuilder.AppendFormat(" and u.F_OrganizeId='{0}'", OperatorProvider.Provider.GetCurrent().CompanyId);
                }
            }
            if (statue != null)
            {
                sqlBuilder.AppendFormat(" and t.statue={0} ", statue);
            }
            if (!string.IsNullOrEmpty(creator_user_name))
            {
                sqlBuilder.AppendFormat(" and u.F_RealName like '%{0}%' ", creator_user_name);
            }

            IEnumerable<TempleteOverviewModel> retData = null;

            if (pagination == null || pagination.rows == 0)
                retData = service.BasicQueryListT<TempleteOverviewModel>(sqlBuilder.ToString());
            else
                retData = service.FindListBySql<TempleteOverviewModel>(sqlBuilder.ToString(), pagination);

            return retData == null ? null : retData.ToList();
        }
        #endregion


        #region 获取需要自己审核的考核方案
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="templete_type">方案类型</param>
        /// <returns></returns>
        public List<TempleteOverviewModel> GetCheckTempleteOverview(int? templete_type, string templete_name, int? statue, int? templete_check_statue, string checker, string creator_user_name, Pagination pagination)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT c.id,t.id AS templete_id,t.templete_name, t.templete_check_statue,t.statue AS statue,t.templete_type,
                                c.check_suggest AS back_reason, t.creator_user_id, u.F_RealName AS creator_user_name,r.F_FullName AS creator_user_duty, o.F_FullName AS creator_user_org, t.last_modify_time, t.create_time ");
            sqlBuilder.Append(@"FROM t_kpi_templete t
                                LEFT JOIN t_kpi_templete_check c
                                ON t.id = c.templete_id
                                LEFT JOIN sys_user u
                                ON u.F_Id=t.creator_user_id
                                LEFT JOIN sys_role r
                                ON r.F_id=u.F_DutyId
                                LEFT JOIN sys_organize o
                                ON o.F_Id=u.F_DepartmentId ");
            sqlBuilder.AppendFormat(" where 1=1 ");
            if (!string.IsNullOrEmpty(templete_name))
            {
                sqlBuilder.AppendFormat(" and t.templete_name like '%{0}%' ", templete_name);
            }
            if (templete_check_statue != null)
            {
                sqlBuilder.AppendFormat(" and t.templete_check_statue={0} ", templete_check_statue);
            }
            if (templete_type != null)
            {
                sqlBuilder.AppendFormat(" and t.templete_type={0} ", templete_type);
            }
            if (statue != null)
            {
                sqlBuilder.AppendFormat(" and t.statue={0} ", statue);
            }
            if (!string.IsNullOrEmpty(checker))
            {
                sqlBuilder.AppendFormat(" and c.checker='{0}'", checker);
            }
            if (!string.IsNullOrEmpty(creator_user_name))
            {
                sqlBuilder.AppendFormat(" and u.F_RealName like '%{0}%' ", creator_user_name);
            }

            IEnumerable<TempleteOverviewModel> retData = null;

            if (pagination == null || pagination.rows == 0)
                retData = service.BasicQueryListT<TempleteOverviewModel>(sqlBuilder.ToString());
            else
                retData = service.FindListBySql<TempleteOverviewModel>(sqlBuilder.ToString(), pagination);

            return retData.ToList();
        }
        #endregion

        #region 获取某个考核方案详情
        /// <summary>
        /// 获取系统默认考核方案
        /// </summary>
        /// <param name="id">分页</param>
        /// <param name="templete_type">方案类型</param>
        /// <returns></returns>
        public List<TempleteOverviewModel> GetTempleteOverviewByTempleteId(string templete_id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT c.id AS id,t.id AS templete_id,t.templete_name, t.templete_check_statue,t.statue AS statue,t.templete_type,cm.dimension_id,d.dimension_name,cm.detail_id,dd.detail_name,dd.formule AS indicator_formula_name,
                                m.check_method_name,m.check_method_define,cm.base_score,cm.allow_beyond_limit,
                                c.check_suggest AS back_reason, t.creator_user_id,u.F_RealName AS creator_user_name,r.F_FullName AS creator_user_duty, o.F_FullName AS creator_user_org, 
                                uu.F_RealName AS checker_name,c.checker AS checker_id,
                                t.last_modify_time, t.create_time ");
            sqlBuilder.Append(@"FROM t_kpi_templete t
                                LEFT JOIN t_kpi_templete_check c
                                ON t.id = c.templete_id
                                LEFT JOIN t_kpi_templete_compostion cm
                                ON t.id = cm.templete_id
                                LEFT JOIN t_kpi_dimension_detail dd
                                ON dd.id = cm.detail_id 
                                LEFT JOIN t_kpi_dimension d
                                ON d.id=cm.dimension_id
                                LEFT JOIN t_check_method m
                                ON dd.method_id=m.id 
                                LEFT JOIN sys_user u
                                ON u.F_Id=t.creator_user_id
                                LEFT JOIN sys_role r
                                ON r.F_id=u.F_DutyId
                                LEFT JOIN sys_organize o
                                ON o.F_Id=u.F_DepartmentId 
                                LEFT JOIN sys_user uu
                                ON uu.F_id=c.checker ");
            sqlBuilder.AppendFormat("WHERE t.id='{0}' ", templete_id);

            IEnumerable<TempleteOverviewModel> retData = null;
            retData = service.BasicQueryListT<TempleteOverviewModel>(sqlBuilder.ToString());
            return retData == null ? null : retData.ToList();
        }
        #endregion

        public List<TempleteEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public TempleteEntity GetForm(string keyValue)
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
        public void SubmitForm(TempleteEntity TempleteEntity, string keyValue)
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
        public TempleteEntity SubmitFormEx(TempleteEntity templeteEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                templeteEntity.Modify(keyValue);
                service.Update(templeteEntity);
            }
            else
            {
                templeteEntity.Create();
                service.Insert(templeteEntity);
            }
            return templeteEntity;
        }

        public TempleteEntity GetTempleteById(string id)
        {
            var expression = ExtLinq.True<TempleteEntity>();
            if (!string.IsNullOrEmpty(id))
            {
                expression = expression.And(r => r.id == id);
            }
            return service.FindEntity(expression);
        }

        public bool IsExists(string templete_name)
        {
            string sql = string.Format("SELECT * FROM t_kpi_templete WHERE templete_name= '{0}'", templete_name);
            return service.FindList(sql).Count > 0;
        }
    }
}
