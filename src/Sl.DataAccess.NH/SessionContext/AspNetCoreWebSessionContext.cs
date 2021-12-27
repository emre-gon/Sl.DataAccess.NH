using Microsoft.AspNetCore.Http;
using NHibernate.Context;
using NHibernate.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH.SessionContext
{
    [Serializable]
    public class AspNetCoreWebSessionContext : MapBasedSessionContext
    {
        private const string SessionFactoryMapKey = "NHibernate.Context.AspNetCoreWebSessionContext.SessionFactoryMapKey";
         

        public AspNetCoreWebSessionContext(ISessionFactoryImplementor factory) : base(factory)
        {
        }

        protected override IDictionary GetMap()
        {
            if (NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext == null)
                throw new Exception("No HttpContext is known for current thread.");

            return NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items[SessionFactoryMapKey] as IDictionary;
        }

        protected override void SetMap(IDictionary value)
        {
            NHibernateWebSessionMiddleWare.HttpContextAccessor.HttpContext.Items[SessionFactoryMapKey] = value;
        }
    }
}
