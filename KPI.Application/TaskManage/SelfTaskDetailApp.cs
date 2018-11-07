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
    public class SelfTaskDetailApp
    {
        private ISelfTaskDetailRepository service = new SelfTaskDetailRepository();

        public List<SelfTaskDetailEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public SelfTaskDetailEntity GetForm(string keyValue)
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
        public void SubmitForm(SelfTaskDetailEntity selfTaskDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                selfTaskDetailEntity.Modify(keyValue);
                service.Update(selfTaskDetailEntity);
            }
            else
            {
                selfTaskDetailEntity.Create();
                service.Insert(selfTaskDetailEntity);
            }
        }
        public SelfTaskDetailEntity SubmitFormEx(SelfTaskDetailEntity selfTaskDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                selfTaskDetailEntity.Modify(keyValue);
                service.Update(selfTaskDetailEntity);
            }
            else
            {
                selfTaskDetailEntity.Create();
                service.Insert(selfTaskDetailEntity);
            }
            return selfTaskDetailEntity;
        }

        public List<SelfTaskDetailEntity> GetPersonalTask(string person, int task_type, int task_statue, string task_object, string pagination)
        {
            Pagination p = Json.ToObject<Pagination>(pagination);
            if (p.rows == 0)
            {
                //分页rows=0 获取全部
                return GetList();
            }
            var expression = ExtLinq.True<SelfTaskDetailEntity>();
            if (!string.IsNullOrEmpty(person))
            {
                expression = expression.And(r => r.person == person);
            }
            if (task_type != -1)
            {
                expression = expression.And(r => r.task_type == task_type);
            }
            if (task_statue != -1)
            {
                expression = expression.And(r => r.task_statue == task_statue);
            }
            if (!string.IsNullOrEmpty(task_object))
            {
                expression = expression.And(r => r.task_object == task_object);
            }
            return service.FindList(expression, p);
        }

        public List<SelfTaskDetailEntity> GetListByAttach(string attach)
        {
            string sql = string.Format("select * from t_self_task_detail where attach = '{0}'", attach);
            return service.FindList(sql);
        }

        public List<TaskMode> GetSelfAllTasks(string person)
        {
            string sql = string.Format("SELECT task_type,COUNT(task_type) as task_count FROM t_self_task_detail WHERE person = '{0}' and task_statue = 1 GROUP BY task_type", person);
            return service.BasicQueryListT<TaskMode>(sql);
        }

        public List<SelfTaskDetailEntity> GetSelfTaskByTaskObj(string taskObject)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT * FROM t_self_task_detail ");
            if (!String.IsNullOrEmpty(taskObject))
            {
                sqlBuilder.AppendFormat("WHERE task_object= '{0}'", taskObject);
            }
            return service.FindList(sqlBuilder.ToString());
        }


        public SelfTaskDetailEntity GetPersonalTaskByTaskObj(string taskObject,string checker)
        {
            var expression = ExtLinq.True<SelfTaskDetailEntity>();
            expression = expression.And(t => t.task_object == taskObject);
            expression = expression.And(t => t.person == checker);
            return service.FindEntity(expression);
        }

        public SelfTaskDetailEntity GetByPersonAttachPersonTaskObjTaskType(string attach, string person,string task_object,int task_type)
        {
            var expression = ExtLinq.True<SelfTaskDetailEntity>();
            expression = expression.And(t => t.attach == attach);
            expression = expression.And(t => t.person == person);
            expression = expression.And(t => t.task_object == task_object);
            expression = expression.And(t => t.task_type == task_type);
            return service.FindEntity(expression);
        }
        public SelfTaskDetailEntity GetByPersonAttachTaskObjTaskType(string attach, string task_object, int task_type)
        {
            var expression = ExtLinq.True<SelfTaskDetailEntity>();
            expression = expression.And(t => t.attach == attach);
            expression = expression.And(t => t.task_object == task_object);
            expression = expression.And(t => t.task_type == task_type);
            return service.FindEntity(expression);
        }
    }
}
