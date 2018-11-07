using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Data;
using KPI.Domain.Entity.AssessmentManage;
using KPI.Domain.IRepository;
using MySql.Data.MySqlClient;

namespace KPI.Repository.AssessmentManage
{
    public class AssessmentDetailRepository : RepositoryBase<AssessmentDetailEntity>, IAssessmentDetailRepository
    {
        
    }
    
}
