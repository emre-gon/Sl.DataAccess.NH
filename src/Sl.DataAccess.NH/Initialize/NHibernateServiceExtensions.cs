using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sl.DataAccess.NH.AutoMap;
using Sl.DataAccess.NH.SessionContext;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Sl.DataAccess.NH
{
    public static class NHibernateServiceExtensions
    {
        public static IServiceCollection AddNHibernateSession(this IServiceCollection builder,
            Assembly DomainAssembly, IPersistenceConfigurer DBConfig,
            SessionContextType SessionContextType,
            IAuditService AuditService)
        {
            AddNHibernateSession(builder, DomainAssembly, DBConfig, SessionContextType, AuditService, DBSchemaUpdateMode.Do_Nothing);
            return builder;
        }
        public static IServiceCollection AddNHibernateSession(this IServiceCollection builder,
            Assembly DomainAssembly, IPersistenceConfigurer DBConfig,
            SessionContextType SessionContextType,
            IAuditService AuditService,
            DBSchemaUpdateMode DBSchemaUpdateMode)
        {
            SlSession.ConfigureSessionFactory(DomainAssembly, DBConfig, SessionContextType, AuditService, DBSchemaUpdateMode);
            return builder;
        }

        public static IApplicationBuilder AddNHibernateWebSession(this IApplicationBuilder builder, IHttpContextAccessor HttpContextAccessor)
        {
            NHibernateWebSessionMiddleWare.HttpContextAccessor = HttpContextAccessor;
            return builder.UseMiddleware<NHibernateWebSessionMiddleWare>();
        }
    }
}
