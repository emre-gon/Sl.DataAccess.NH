using System;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    /// <summary>
    /// Nvarchar -> varchar
    /// </summary>
    public class AnsiStringAttribute : Attribute
    {
    }
}
