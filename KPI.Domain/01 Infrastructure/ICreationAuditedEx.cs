
using System;

namespace KPI.Domain
{
    public interface ICreationAuditedEx
    {
        string id { get; set; }
        string creator_user_id { get; set; }
        DateTime? create_time { get; set; }
    }
}