using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Sl.DataAccess.NH.AutoMap.NHEventListeners;
using Microsoft.AspNetCore.Http;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Event;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Sl.DataAccess.NH.SessionContext;
using Sl.DataAccess.NH.AutoMap.AutoMapperConventions;
using Sl.DataAccess.NH.AutoMap.CustomMappingSteps;

namespace Sl.DataAccess.NH.AutoMap
{
    public enum DBSchemaUpdateMode
    {
        Update_Tables,
        Drop_And_Recreate_Tables,
        Do_Nothing
    }

    public enum SessionContextType
    {
        Web,
        Web_Old, //pre katana
        Hybrid,
        ThreadStatic,
        ThreadLocal
    }
    public static class NHAutoMapper
    {

        private static void AddTriggerEventHandlers(Configuration config)
        {
            config.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[]
            {
                new BeforeInsertEventListener()
            };
            config.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[]
            {
                new AfterInsertEventListener()
            };

            config.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[]
            {
                new BeforeUpdateEventListener()
            };
            config.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[]
            {
                new AfterUpdateEventListener()
            };

            config.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[]
            {
                new BeforeDeleteEventListener()
            };
            config.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[]
            {
                new AfterDeleteEventListener()
            };
        }

        private static void BuildSchema(Configuration config, DBSchemaUpdateMode SchemaUpdateMode)
        {
            switch (SchemaUpdateMode)
            {
                case DBSchemaUpdateMode.Update_Tables:
                    new SchemaUpdate(config).Execute(false, true);
                    break;
                case DBSchemaUpdateMode.Drop_And_Recreate_Tables:
                    new SchemaExport(config).Execute(false, true, true);
                    new SchemaExport(config).Execute(false, true, false);
                    break;
                default:
                    break;
            }            
        }
        

        internal static ISessionFactory CreateSessionFactory(Assembly DomainAssembly, 
            IPersistenceConfigurer DBConfig,
            SessionContextType SessionContextType,
            DBSchemaUpdateMode SchemaUpdateMode)
        {          
            var autoMapper = FluentNHibernate.Automapping.AutoMap
                .Assembly(DomainAssembly, new NHibernateAutoMappingConfiguration())
                    .Conventions.Add<TableNameConvention>()
                    .Conventions.Add<ColumnNameConvention>()
                    .Conventions.Add<JsonColumnConvention>(new JsonColumnConvention(DBConfig))
                    .Conventions.Add<CascadeConvention>()
                    .Conventions.Add<AnsiStringConvention>()
                    .Conventions.Add<ColumnNullConvention>()
                    .Conventions.Add<NotNullReferenceConvention>()
                    .Conventions.Add<CheckReserverdKeywordsConvention>()
                    .Conventions.Add<StringLengthConvention>()
                    .Conventions.Add<UniqueConvention>()
                    .Conventions.Add<UniqueRefConvention>()
                    .Conventions.Add<IndexConvention>()
                    .Conventions.Add<IndexRefConvention>()
                    .Conventions.Add<DateColumnConvention>(new DateColumnConvention(DBConfig))
                    .Conventions.Add<DateTypeConvention>();





            var conf = Fluently.Configure(new Configuration()
                .SetNamingStrategy(new CustomNamingStrategy()))
                .Database(DBConfig)                
                .Mappings(m => m.AutoMappings.Add(autoMapper));


            switch (SessionContextType)
            {
                case SessionContextType.ThreadStatic:
                    conf = conf.CurrentSessionContext<ThreadStaticSessionContext>();
                    break;
                case SessionContextType.ThreadLocal:
                    conf = conf.CurrentSessionContext<ThreadLocalSessionContext>();
                    break;
                case SessionContextType.Web:
                    conf = conf.CurrentSessionContext<AspNetCoreWebSessionContext>();
                    break;
                case SessionContextType.Web_Old:
                    conf = conf.CurrentSessionContext<WebSessionContext>();
                    break;
                case SessionContextType.Hybrid:
                    conf = conf.CurrentSessionContext<HybridWebSessionContext>();
                    break;
            }

            var SessionFactory = conf
                .ExposeConfiguration(f=> BuildSchema(f, SchemaUpdateMode))
                .ExposeConfiguration(AddTriggerEventHandlers)
                .BuildSessionFactory();
            
            return SessionFactory;
        }
    }
}
