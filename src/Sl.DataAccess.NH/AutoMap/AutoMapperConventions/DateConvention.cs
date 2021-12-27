using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    /// <summary>
    /// datetime2 -> date
    /// </summary>
    public class DateTypeConvention : AttributePropertyConvention<DataTypeAttribute>
    {
        protected override void Apply(DataTypeAttribute attribute, IPropertyInstance instance)
        {
            if(attribute.DataType == DataType.Date)
            {
                if (instance.Property.PropertyType == typeof(DateTime)
                    || instance.Property.PropertyType == typeof(DateTime?))
                    instance.CustomType("Date");
            }
            else if(attribute.DataType == DataType.Time)
            {
                if (instance.Property.PropertyType == typeof(DateTime)
                    || instance.Property.PropertyType == typeof(DateTime?))
                    instance.CustomType("Time");
            }
        }
    }
}
