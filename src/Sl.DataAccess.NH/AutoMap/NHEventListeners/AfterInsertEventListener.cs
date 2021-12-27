using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.AutoMap.NHEventListeners
{
    class AfterInsertEventListener : IPostInsertEventListener
    {
        public void OnPostInsert(PostInsertEvent @event)
        {
            (@event.Entity as ITableBase).AfterInsert();
        }

        public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
        {
            return new Task(() => OnPostInsert(@event));
        }
    }
}
