using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Sl.DataAccess.NH.CustomAttributes;
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

            var keyColumn = keyColumns.First();

            bool shouldAutoGenerate;

            var autoGenerateAttr = keyColumn.GetCustomAttribute<AutoGenerateAttribute>();
            if (autoGenerateAttr != null)
            {
                shouldAutoGenerate = autoGenerateAttr.ShouldAutoGenerate;
            }
            else
            {
                switch (Type.GetTypeCode(keyColumn.PropertyType))
                {
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        shouldAutoGenerate = true;
                        break;
                    case TypeCode.Object:
                        if (keyColumn.PropertyType == typeof(Guid))
                        {
                            shouldAutoGenerate = true;
                        }
                        else
                        {
                            shouldAutoGenerate = false;
                        }
                        break;
                    default:
                        shouldAutoGenerate = false;
                        break;
                }
            }

            if (shouldAutoGenerate)
            {
                if (keyColumn.PropertyType == typeof(Guid))
                {
                    instance.GeneratedBy.GuidNative();
                }
                else
                {
                    instance.GeneratedBy.Identity();
                }                
            }
            else
            {
                instance.GeneratedBy.Assigned();
            }
        }
    }
}
