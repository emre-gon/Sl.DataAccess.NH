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
    public class KeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            var keyColumns = instance.EntityType.GetProperties()
                .Where(f => f.IsDefined(typeof(KeyAttribute)));

            if (keyColumns.Count() != 1)
            {
                return;
            }


            var propType = keyColumns.First().PropertyType;

            bool shouldIncrement;
            switch (Type.GetTypeCode(propType))
            {
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    shouldIncrement = true;
                    break;
                default:
                    shouldIncrement = false;
                    break;
            }


            if (shouldIncrement)
            {
                instance.GeneratedBy.Identity();
            }


            if (propType == typeof(Guid))
            {
                instance.GeneratedBy.GuidNative();
            }
        }
    }
}
