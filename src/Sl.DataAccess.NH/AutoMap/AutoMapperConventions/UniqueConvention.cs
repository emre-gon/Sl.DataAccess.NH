using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    public class UniqueRefConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            if (instance.Property.MemberInfo
                  .IsDefined(typeof(UniqueAttribute), false))
            {
                UniqueConvention.ConventionQueue.Add(instance);
            }


            var table = instance.Property.DeclaringType;
            var propName = instance.Property.Name;

            foreach (var otherProp in table.GetProperties().Where(f => f.IsDefined(typeof(UniqueAttribute), false)))
            {
                var otherAttributes = otherProp.GetCustomAttributes<UniqueAttribute>(false);

                foreach (var attr in otherAttributes)
                {
                    if (!attr.PartnerColumns.Contains(propName))
                    {
                        continue;
                    }

                    UniqueConvention.ConventionQueue.Add(instance);

                }
            }
        }
    }

    public class UniqueConvention : IPropertyConvention
    {
        public static readonly IndexPropertyConventionQueue<UniqueAttribute> ConventionQueue = new IndexPropertyConventionQueue<UniqueAttribute>();

        public void Apply(IPropertyInstance instance)
        {            
            if(instance.Property.MemberInfo
                .IsDefined(typeof(UniqueAttribute), false))
            {
                ConventionQueue.Add(instance);
            }


            var table = instance.Property.DeclaringType;
            var propName = instance.Property.Name;

            foreach (var otherProp in table.GetProperties().Where(f => f.IsDefined(typeof(UniqueAttribute), false)))
            {
                var otherAttributes = otherProp.GetCustomAttributes<UniqueAttribute>(false);

                foreach (var attr in otherAttributes)
                {
                    if (!attr.PartnerColumns.Contains(propName))
                    {
                        continue;
                    }

                    ConventionQueue.Add(instance);

                }
            }
        }
    }
}
