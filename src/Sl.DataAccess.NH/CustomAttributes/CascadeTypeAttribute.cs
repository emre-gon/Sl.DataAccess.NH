using System;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH
{
    public enum CascadeType
    {
        All,
        SaveUpdate,
        Merge,
        Delete,
        None
    }



    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CascadeTypeAttribute : Attribute
    {
        public CascadeTypeAttribute(CascadeType cascadeType)
        {
            this.CascadeType = cascadeType;
        }

        public CascadeType CascadeType { get; }
    }
}
