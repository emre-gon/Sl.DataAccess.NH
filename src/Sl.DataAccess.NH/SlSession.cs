using Sl.DataAccess.NH.AutoMap;
using Microsoft.AspNetCore.Http;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using NHibernate;
using Sl.DataAccess.NH.SessionContext;
using System.Reflection;
using FluentNHibernate.Cfg.Db;

namespace Sl.DataAccess.NH
{
    public partial class SlSession
    {
        internal static ISessionFactory SessionFactory { get; private set; }

        internal static IAuditService AuditService { get; private set; }


        public static void ConfigureSessionFactory(Assembly domainAssembly,
            IPersistenceConfigurer dBConfig, SessionContextType SessionContextType,
            IAuditService auditService,            
            DBSchemaUpdateMode DBSchemaUpdateMode)
        {
            AuditService = auditService;
            SessionFactory = NHAutoMapper.CreateSessionFactory(domainAssembly,
                dBConfig, SessionContextType, DBSchemaUpdateMode);
        }



        public static NHibernate.ISession NH
        {
            get
            {
                try
                {
                    var session = SessionFactory.GetCurrentSession();
                    if(!session.IsOpen)
                    {
                        session = SessionFactory.OpenSession();
                        CurrentSessionContext.Bind(session);
                    }
                    return session;
                }
                catch(HibernateException exc)
                {
                    if (exc.Message == "No session bound to the current context")
                    {
                        
                        var session = SessionFactory.OpenSession();
                        CurrentSessionContext.Bind(session);
                        return SessionFactory.GetCurrentSession();
                    }

                    throw exc;
                }
              
            }
        }
    }
}
