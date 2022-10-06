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
    class BeforeUpdateEventListener : IPreUpdateEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var tb = @event.Entity as ITableBase;

            ValidationContext cont = new ValidationContext(tb);
            Validator.ValidateObject(tb, cont);

            tb.CustomVerify();


            var tbAudit = @event.Entity as TableBase;
            if (tbAudit != null)
            {
                var time = DateTime.UtcNow;
                var userId = SlSession.AuditService?.GetUserID();

                Set(@event.Persister, @event.State, "LastUpdatedAt", time);
                tbAudit.LastUpdatedAt = time;

                Set(@event.Persister, @event.State, "LastUpdatedBy", userId);
                tbAudit.LastUpdatedBy = userId;
            }


            tb.BeforeUpdate();
            return false;
        }

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            return new Task<bool>(() => OnPreUpdate(@event));
        }
    }
}
