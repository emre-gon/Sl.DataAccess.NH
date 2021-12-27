using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDomain
{
    public class AppValidation: IComponent
    {
        public virtual bool? IsValidated { get; set; }

        public virtual string ValidatedBy { get; set; }
    }
}
