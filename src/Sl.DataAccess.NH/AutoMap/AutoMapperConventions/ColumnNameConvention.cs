using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ColumnNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public ColumnNameAttribute(string Name)
        {
            this.Name = Name;
        }
    }



    public class ColumnNameConvention : AttributePropertyConvention<ColumnNameAttribute>
    {
        protected override void Apply(ColumnNameAttribute attribute, IPropertyInstance instance)
        {
            instance.Column(attribute.Name);
        }


    }
}