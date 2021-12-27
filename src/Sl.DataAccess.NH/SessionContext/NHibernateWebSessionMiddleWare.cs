using FluentNHibernate.Cfg.Db;
using Sl.DataAccess.NH.AutoMap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.SessionContext
{
    public class NHibernateWebSessionMiddleWare
    {

        private static IHttpContextAccessor _httpContextAccessor;
        public static IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                return _httpContextAccessor;
            }
            set
            {
                if (_httpContextAccessor != null)
                    throw new Exception("HttpContextAccessor can only bet once during startup.");
                _httpContextAccessor = value;
            }
        }



        private readonly RequestDelegate _next;

        public NHibernateWebSessionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            finally
            {
                var session = NHibernate.Context.CurrentSessionContext.Unbind(SlSession.SessionFactory);
                if (session != null)
                {
                    session.Close();
                    session.Dispose();
                }
            }

        }
    }

}
