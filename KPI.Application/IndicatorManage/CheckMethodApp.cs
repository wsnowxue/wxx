using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Repository.IndicatorManage;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.IRepository;
using KPI.Code;

namespace KPI.Application.IndicatorManage
{
    public class CheckMethodApp
    {
        private ICheckMethodRepository service = new CheckMethodRepository();

        public List<CheckMethodEntity> GetItemList(string check_method_name, string check_method_define,int? check_method_statue, Pagination p)
        {
            if (p.rows == 0 || p == null)
            {
                //分页rows=0 获取全部
                return GetCheckMethodByStatue(check_method_statue);
            }
            var expression = ExtLinq.True<CheckMethodEntity>();
            if (!string.IsNullOrEmpty(check_method_name))
            {
                expression = expression.And(r => r.check_method_name.Contains(check_method_name));
            }
            if (!string.IsNullOrEmpty(check_method_define))
            {
                expression = expression.And(r => r.check_method_define.Contains(check_method_define));
            }
            if (check_method_statue!=null)
            {
                expression = expression.And(r => r.check_method_statue== check_method_statue);
            }
            return service.FindList(expression, p);
        }
        

            public List<CheckMethodEntity> GetCheckMethodByStatue(int? statue)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT * FROM t_check_method ");
            if (statue != null)
            {
                sqlBuilder.AppendFormat("WHERE check_method_statue= {0}", statue);
            }
            return service.FindList(sqlBuilder.ToString());
        }

        public List<CheckMethodEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.create_time).ToList();
        }
        public CheckMethodEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public CheckMethodEntity GetByCheckMethodName(string check_method)
        {
            var expression = ExtLinq.True<CheckMethodEntity>();
            expression = expression.And(r => r.check_method_name == check_method);
            return service.FindEntity(expression);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.id == keyValue);
        }
        public void SubmitForm(CheckMethodEntity checkMethodEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                checkMethodEntity.Modify(keyValue);
                service.Update(checkMethodEntity);
            }
            else
            {
                checkMethodEntity.Create();
                service.Insert(checkMethodEntity);
            }
        }

        public bool IsExists(string check_method_name)
        {
            string sql = string.Format("select * from t_check_method where check_method_name = '{0}'", check_method_name);
            return service.FindList(sql).Count > 0;
        }
    }
}
