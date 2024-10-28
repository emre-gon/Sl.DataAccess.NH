using Sl.DataAccess.NH.AutoMap;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using NHibernate;
using System.Reflection;
using FluentNHibernate.Cfg.Db;

namespace Sl.DataAccess.NH
{
    public partial class SlSession
    {
        internal static ISessionFactory DefaultSessionFactory
        {
            get
            {
                return SessionFactories["Default"];
            }
        }

        internal static IAuditService AuditService { get; private set; }

        internal static Dictionary<string, ISessionFactory> SessionFactories { get; private set; }

        public static void ConfigureSessionFactory(Assembly domainAssembly,
            IPersistenceConfigurer dBConfig, SessionContextType SessionContextType,
            IAuditService auditService,            
            DBSchemaUpdateMode DBSchemaUpdateMode)
        {
            ConfigureSessionFactory("Default", domainAssembly, dBConfig, SessionContextType, auditService, DBSchemaUpdateMode);
        }

        public static void ConfigureSessionFactory(string SessionFactoryName, Assembly domainAssembly,
            IPersistenceConfigurer dBConfig, SessionContextType SessionContextType,
            IAuditService auditService,
            DBSchemaUpdateMode DBSchemaUpdateMode)
        {
            AuditService = auditService;
            var sfConfig = NHAutoMapper.GetFluentConfiguration(domainAssembly,
                dBConfig, SessionContextType, DBSchemaUpdateMode);
            SessionFactories[SessionFactoryName] = sfConfig.BuildSessionFactory();
        }

        public static NHibernate.ISession NHSession(string SessionFactoryKey)
        {
            try
            {
                var session = SessionFactories[SessionFactoryKey].GetCurrentSession();
                if (!session.IsOpen)
                {
                    session = SessionFactories[SessionFactoryKey].OpenSession();
                    CurrentSessionContext.Bind(session);
                }
                return session;
            }
            catch (HibernateException exc)
            {
                if (exc.Message == "No session bound to the current context")
                {

                    var session = SessionFactories[SessionFactoryKey].OpenSession();
                    CurrentSessionContext.Bind(session);
                    return SessionFactories[SessionFactoryKey].GetCurrentSession();
                }

                throw exc;
            }            
        }

        public static NHibernate.ISession NH
        {
            get
            {
                return NHSession("Default");
            }
        }
    }
}
