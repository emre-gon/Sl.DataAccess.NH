using Microsoft.AspNetCore.Http;
using NHibernate.Context;
using NHibernate.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sl.DataAccess.NH.SessionContext
{
    [Serializable]
    public class HybridWebSessionContext : MapBasedSessionContext
    {
        private const string SessionFactoryMapKey = "NHibernate.Context.HybridSessionContext.SessionFactoryMapKey";
        private const string ManagedThreadIdMapKey = "NHibernate.Context.HybridSessionContext.ManagedThreadId";

        [ThreadStatic]
        private static IDictionary threadStaticMap;

        public HybridWebSessionContext(ISessionFactoryImplementor factory) : base(factory)
        {
        }

        private bool NeedsThreadStatic()
        {
            if (NHibernateWebSessionMiddleWare.HttpContextAccessor?.HttpContext == null)
            {
                return true;
            }

            if(!NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items.ContainsKey(ManagedThreadIdMapKey))
            {//ilk kez yaratılıyorsa web'e yollansın
                return false;
            }

            int threadId = (int)NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items[ManagedThreadIdMapKey];
            if (Thread.CurrentThread.ManagedThreadId != threadId)
            {
                return true;
            }

            return false;
        }

        protected override IDictionary GetMap()
        {
            if (NeedsThreadStatic())
                return threadStaticMap;
            else
                return NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items[SessionFactoryMapKey] as IDictionary;
        }

        protected override void SetMap(IDictionary value)
        {
            if(NeedsThreadStatic())
            {
                threadStaticMap = value;
            }
            else
            {
                NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items[SessionFactoryMapKey] = value;
                NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items[ManagedThreadIdMapKey] = Thread.CurrentThread.ManagedThreadId;
            }
        }
    }
}
