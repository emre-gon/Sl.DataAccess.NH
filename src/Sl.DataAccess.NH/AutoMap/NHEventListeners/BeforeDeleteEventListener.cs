using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.AutoMap.NHEventListeners
{
    class BeforeDeleteEventListener : IPreDeleteEventListener
    {
        public bool OnPreDelete(PreDeleteEvent @event)
        {
            (@event.Entity as ITableBase).BeforeDelete();
            return false;
        }

        public Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
        {
            return new Task<bool>(() => OnPreDelete(@event));
        }
    }
}
