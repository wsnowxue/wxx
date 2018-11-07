using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Data;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.IRepository;
using MySql.Data.MySqlClient;

namespace KPI.Repository.IndicatorManage
{
    public class IndicatorsDefineRepository : RepositoryBase<IndicatorsDefineEntity>, IIndicatorsDefineRepository
    {
        public List<IndicatorsDefineEntity> GetItemList(string indicator_name, string indicator_Define, string count_method, int start_index, int total)
        {
            start_index = start_index -1;
            StringBuilder strSql = new StringBuilder();
            
            strSql.Append(@"SELECT  i.*
                            FROM    t_indicators_Define i
                            WHERE   1=1 ");

            if (!string.IsNullOrEmpty(indicator_name))
            {
                strSql.Append(@" AND i.indicator_name like '%"+ @indicator_name + "%'");
            }

            if (!string.IsNullOrEmpty(indicator_Define))
            {
                strSql.Append(@" AND i.indicator_Define like '%" + @indicator_Define + "%'");
            }

            if (!string.IsNullOrEmpty(count_method))
            {
                strSql.Append(@" AND i.indicator_count_method like +  '%" + @count_method+ "%'");
            }

            strSql.Append(@" LIMIT   @start_index,@total");
            DbParameter[] parameter = {
                new MySqlParameter("@indicator_name", indicator_name),
                new MySqlParameter("@indicator_Define", indicator_Define),
                new MySqlParameter("@count_method", count_method),
                new MySqlParameter("@start_index", start_index),
                new MySqlParameter("@total", total)
            };



            return this.FindList(strSql.ToString(), parameter);
        }
    }
    
}
