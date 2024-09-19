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
        public static IApplicationBuilder AddNHibernateWebSession(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NHibernateWebSessionMiddleWare>();
        }
    }
}
