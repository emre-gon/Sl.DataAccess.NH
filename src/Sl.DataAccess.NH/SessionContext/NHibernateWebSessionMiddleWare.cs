using Microsoft.AspNetCore.Http;
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
            if (HttpContextAccessor != null)
            {
                HttpContextAccessor = _httpContextAccessor;
            }            
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            finally
            {
                var session = NHibernate.Context.CurrentSessionContext.Unbind(SlSession.DefaultSessionFactory);
                if (session != null)
                {
                    session.Close();
                    session.Dispose();
                }
            }
        }
    }
}
