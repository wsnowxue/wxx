
using System;

namespace KPI.Domain
{
    public interface IModificationAuditedEx
    {
        string id{ get; set; }
        DateTime? last_modify_time { get; set; }
    }
}