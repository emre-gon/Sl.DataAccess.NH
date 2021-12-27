using Microsoft.AspNetCore.Http;
using Sl.DataAccess.NH.SessionContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace Sl.DataAccess.NH.Audit
{
    public class ClaimsIdentityAuditService : IAuditService
    {
        public HttpContext HttpContext
        {
            get
            {
                return NHibernateWebSessionMiddleWare.HttpContextAccessor?.HttpContext;
            }
        }



        public IEnumerable<string> Roles
        {
            get
            {
                if (!IsAuthenticated)
                    return null;

                return Claims.Where(f => f.Type == ClaimTypes.Role).Select(f => f.Value);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return Claims != null;
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                var identity = (ClaimsIdentity)HttpContext?.User?.Identity;
                return identity?.Claims;

                //if (HttpContext == null)
                //{
                //    //TODO: Web User'ın bir işlemi background thread'de koşabilir
                //    //Bu durumda HttpContext yok, peki Identity ne olacak?
                //    //Paralel thread açarken identity de set mi edilmeli ?
                //    var identity = (ClaimsIdentity)Thread.CurrentPrincipal?.Identity;
                //    return identity?.Claims;
                //}
            }
        }


        public string UserName
        {
            get
            {
                if (!IsAuthenticated)
                    return null;

                return Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value;
            }
        }


        public int? GetUserID()
        {
            if (!IsAuthenticated)
                return null;

            var val = Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value;

            return val == null ? null : (int?)int.Parse(val);
        }
    }
}
