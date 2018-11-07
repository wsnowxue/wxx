using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Data;
using KPI.Domain.Entity.IndicatorManage;
using KPI.Domain.Entity.TempleteManage;
using KPI.Domain.IRepository;
using MySql.Data.MySqlClient;

namespace KPI.Repository.TempleteManage
{
    public class TempleteCheckRepository : RepositoryBase<TempleteCheckEntity>, ITempleteCheckRepository
    {
        
    }
    
}
