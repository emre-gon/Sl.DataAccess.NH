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
    public class StringLengthConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            var attr = ((PropertyInfo)instance.Property.MemberInfo).GetCustomAttribute<MaxLengthAttribute>();
            if (attr != null)
            {
                instance.Length(attr.Length);
            }
        }
    }
}