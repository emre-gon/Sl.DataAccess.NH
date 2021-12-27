using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.AutoMap.NHEventListeners
{
    class AfterUpdateEventListener : IPostUpdateEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            (@event.Entity as ITableBase).AfterUpdate();
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            return new Task(() => OnPostUpdate(@event));
        }
    }
}
