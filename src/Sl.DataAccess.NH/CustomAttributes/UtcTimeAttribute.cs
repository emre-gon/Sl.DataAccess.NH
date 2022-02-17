using System;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UtcTimeAttribute : Attribute
    {
    }
}
