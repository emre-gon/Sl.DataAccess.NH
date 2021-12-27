using Sl.DataAccess.NH;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.AutoMap.NHEventListeners
{
    class BeforeInsertEventListener : IPreInsertEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            var tb = @event.Entity as ITableBase;

            ValidationContext cont = new ValidationContext(tb);

            Validator.ValidateObject(tb, cont);



            var tbAudit = @event.Entity as TableBaseWithReadAudit;
            if (tbAudit != null)
            {
                var time = DateTime.Now;
                var userId = SlSession.AuditService?.GetUserID();

                if (tbAudit.CreatedAt == default)
                {
                    Set(@event.Persister, @event.State, "CreatedAt", time);
                    tbAudit.CreatedAt = time;
                }

                if (tbAudit.CreatedBy == null)
                {
                    Set(@event.Persister, @event.State, "CreatedBy", userId);
                    tbAudit.CreatedBy = userId;
                }
            }

            tb.CustomVerify();
            tb.BeforeInsert();
            return false;
        }

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            return new Task<bool>(() => OnPreInsert(@event));
        }
    }
}
