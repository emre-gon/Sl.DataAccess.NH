using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    public class IndexRefConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            if (instance.Property.MemberInfo
                  .IsDefined(typeof(IndexAttribute), false))
            {
                IndexConvention.ConventionQueue.Add(instance);
            }


            var table = instance.Property.DeclaringType;
            var propName = instance.Property.Name;

            foreach (var otherProp in table.GetProperties().Where(f => f.IsDefined(typeof(IndexAttribute), false)))
            {
                var otherAttributes = otherProp.GetCustomAttributes<IndexAttribute>(false);

                foreach (var attr in otherAttributes)
                {
                    if (!attr.PartnerColumns.Contains(propName))
                    {
                        continue;
                    }

                    IndexConvention.ConventionQueue.Add(instance);

                }
            }
        }
    }



    public class IndexConvention : IPropertyConvention
    {
        public static readonly IndexPropertyConventionQueue<IndexAttribute> ConventionQueue = new IndexPropertyConventionQueue<IndexAttribute>();

        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.MemberInfo
                .IsDefined(typeof(IndexAttribute), false))
            {
                ConventionQueue.Add(instance);
            }


            var table = instance.Property.DeclaringType;
            var propName = instance.Property.Name;

            foreach (var otherProp in table.GetProperties().Where(f => f.IsDefined(typeof(IndexAttribute), false)))
            {
                var otherAttributes = otherProp.GetCustomAttributes<IndexAttribute>(false);

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
