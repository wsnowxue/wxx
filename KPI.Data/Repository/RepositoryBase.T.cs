
using KPI.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity.Infrastructure;

namespace KPI.Data
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        public KPIDbContext dbcontext = new KPIDbContext();
        public int Insert(TEntity entity)
        {
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return dbcontext.SaveChanges();
        }
        public int Insert(List<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return dbcontext.SaveChanges();
        }
        public int Update(TEntity entity)
        {
            dbcontext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return dbcontext.SaveChanges();
        }
        public int Delete(TEntity entity)
        {
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return dbcontext.SaveChanges();
        }
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return dbcontext.SaveChanges();
        }
        public TEntity FindEntity(object keyValue)
        {
            return dbcontext.Set<TEntity>().Find(keyValue);
        }
        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> IQueryable()
        {
            return dbcontext.Set<TEntity>();
        }
        public IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Set<TEntity>().Where(predicate);
        }
        public List<TEntity> FindList(string strSql)
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList(string strSql, DbParameter[] dbParameter)
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList(Pagination pagination)
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().Where(predicate);
            if (pagination.sord != null && pagination.sidx != null)
            {
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(TEntity), "t");
                    var property = typeof(TEntity).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
            }
            else
            {
                //typeof(SampleClass).GetMethod("Increment"),
                //public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, int count);
                resultExp = Expression.Call(typeof(Queryable).GetMethod("Take"), tempData.Expression);
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }


        public List<T> BasicQueryListT<T>(string sql)
        {
            DbRawSqlQuery<T> result = dbcontext.Database.SqlQuery<T>(sql);
            if (result.Count() > 0)
                return result.ToList();
            return null;
        }

        public T BasicQueryT<T>(string sql)
        {
            DbRawSqlQuery<T> result = dbcontext.Database.SqlQuery<T>(sql);
            if (result.Count() > 0)
                return result.First();
            return default(T);
        }
        public IEnumerable<T> FindListBySql<T>(string strSql, Pagination pagination)
        {
            int total = pagination.records;
            var data = FindListBySql<T>(strSql, pagination.sidx, pagination.sord != null && pagination.sord.ToLower() == "asc" ? true : false, pagination.rows, pagination.page, out total);
            pagination.records = total;
            return data;
        }
        public IEnumerable<T> FindListBySql<T>(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex, out int total)
        {
            return FindListBySql<T>(strSql, null, orderField, isAsc, pageSize, pageIndex, out total);
        }
        public IEnumerable<T> FindListBySql<T>(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex, out int total)
        {
            using (DbConnection conn = new MySqlConnection(DbHelper.connstring))
            {
                StringBuilder sb = new StringBuilder();
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                string OrderBy = "";

                if (!string.IsNullOrEmpty(orderField))
                {
                    if (orderField.ToUpper().IndexOf("ASC") + orderField.ToUpper().IndexOf("DESC") > 0)
                    {
                        OrderBy = " Order By " + orderField;
                    }
                    else
                    {
                        OrderBy = " Order By " + orderField + " " + (isAsc ? "ASC" : "DESC");
                    }
                }
                else
                {
                    OrderBy = " order by (select 0)";
                }
#if SQLSERVER
                sb.Append("Select * From (Select ROW_NUMBER() Over (" + OrderBy + ")");
                sb.Append(" As rowNum, * From (" + strSql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
#endif 
                sb.Append(strSql);
                sb.Append(OrderBy);
                sb.AppendFormat(" limit {0},{1}", num, pageSize);
                total = Convert.ToInt32(new DbHelper(conn).ExecuteScalar("Select Count(1) From (" + strSql + ") As t"));
                IDataReader rdr = new DbHelper(conn).ExecuteReader(sb.ToString());
                return ConvertExtension.IDataReaderToList<T>(rdr);
            }
        }

        public DataTable FindTable(string strSql)
        {
            using (DbConnection conn = new MySqlConnection(DbHelper.connstring))
            {
                var IDataReader = new DbHelper(conn).ExecuteReader(strSql);
                return ConvertExtension.IDataReaderToDataTable(IDataReader);
            }
        }
    }
}
