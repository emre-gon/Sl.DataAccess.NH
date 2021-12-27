using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.AutoMap.NHEventListeners
{
    class AfterDeleteEventListener : IPostDeleteEventListener
    {
        public void OnPostDelete(PostDeleteEvent @event)
        {
            (@event.Entity as ITableBase).AfterDelete();
        }

        public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
        {
            return new Task(() => OnPostDelete(@event));
        }
    }
}
