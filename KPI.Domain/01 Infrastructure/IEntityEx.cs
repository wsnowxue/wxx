
using KPI.Code;
using System;

namespace KPI.Domain
{
    public class IEntityEx<TEntity>
    {
        public void Create()
        {
            var entity = this as ICreationAuditedEx;
            if (string.IsNullOrEmpty(entity.id))
                entity.id = Common.GuId();
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                entity.creator_user_id = LoginInfo.UserId;
            }
            entity.create_time = DateTime.Now;
        }
        public void Modify(string keyValue)
        {
            var entity = this as IModificationAuditedEx;
            entity.id = keyValue;
            entity.last_modify_time = DateTime.Now;
        }
        public void Remove()
        {
            var entity = this as IDeleteAuditedEx;
            entity.delete_mark = 1;
        }
    }
}
