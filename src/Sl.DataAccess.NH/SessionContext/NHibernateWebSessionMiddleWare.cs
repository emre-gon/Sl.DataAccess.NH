using Microsoft.AspNetCore.Http;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace Sl.DataAccess.NH.SessionContext
{
    public class NHibernateWebSessionMiddleWare
    {
        public static IHttpContextAccessor HttpContextAccessor { get; private set; }

        private readonly RequestDelegate _next;

        public NHibernateWebSessionMiddleWare(RequestDelegate next, IHttpContextAccessor _httpContextAccessor)
        {
            _next = next;
            HttpContextAccessor = _httpContextAccessor;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            finally
            {
                foreach (var factory in SlSession.SessionFactories)
                {
                    // A thread-bound session (ThreadStatic, ThreadLocal, Hybrid off its first thread) may
                    // belong to another request that ran on this thread, so only the default factory keeps
                    // its old unbind; sessions bound to the request (Web) are closed for every factory.
                    bool boundToRequest = ((ISessionFactoryImplementor)factory.Value).CurrentSessionContext is AspNetCoreWebSessionContext;
                    if (factory.Key != "Default" && !boundToRequest)
                        continue;

                    var session = NHibernate.Context.CurrentSessionContext.Unbind(factory.Value);
                    if (session != null)
                    {
                        session.Close();
                        session.Dispose();
                    }
                }
            }
        }
    }
}
