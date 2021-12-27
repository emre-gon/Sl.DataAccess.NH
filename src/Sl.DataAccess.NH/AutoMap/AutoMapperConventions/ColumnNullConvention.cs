using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    public class ColumnNullConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (IsPropertyNotNull((PropertyInfo)instance.Property.MemberInfo))
                instance.Not.Nullable();
        }


        public static bool IsPropertyNotNull(PropertyInfo prop)
        {
            return prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(decimal) || prop.PropertyType.IsEnum
                || prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(DateTime)
                || prop.IsDefined(typeof(RequiredAttribute))
                || prop.IsDefined(typeof(KeyAttribute));
            //|| (prop.HasAttributeOfType<MinMultiSelect>() && prop.GetAttributesOfType<MinMultiSelect>().Single().MinCount > 0);
            //TODO:
        }
    }

    public class NotNullReferenceConvention : IReferenceConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IManyToOneInstance instance)
        {
            if (instance.Property.MemberInfo.IsDefined(typeof(RequiredAttribute), false))
                instance.Not.Nullable();                 
        }
    }
}
