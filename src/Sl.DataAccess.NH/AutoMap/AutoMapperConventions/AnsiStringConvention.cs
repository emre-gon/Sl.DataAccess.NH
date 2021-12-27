using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    /// <summary>
    /// Nvarchar -> varchar
    /// </summary>
    public class AnsiStringConvention : AttributePropertyConvention<AnsiStringAttribute>
    {
        protected override void Apply(AnsiStringAttribute attribute, IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(string))
                instance.CustomType("AnsiString");
            else
            {
                throw new Exception($"AnsiString attribute can only be used on string properties." +
                    $" TableName: {instance.Property.DeclaringType.Name}, Property: {instance.Property.Name}");
            }
        }
    }
}
