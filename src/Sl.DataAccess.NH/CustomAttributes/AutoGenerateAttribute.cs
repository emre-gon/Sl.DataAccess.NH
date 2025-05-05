using System;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoGenerateAttribute : Attribute
    {
        public AutoGenerateAttribute(bool shouldAutoGenerate = true)
        {
            this.ShouldAutoGenerate = shouldAutoGenerate;
        }

        public bool ShouldAutoGenerate { get; private set; }
    }
}
