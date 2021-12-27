using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.DataAnnotations
{
    [System.AttributeUsage(System.AttributeTargets.Property,
                      AllowMultiple = false,
                      Inherited = true)]
    public class DBIgnore : Attribute
    {
    }
}
