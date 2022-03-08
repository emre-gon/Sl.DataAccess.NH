using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Json;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH
{
    public class DateColumnConvention : IUserTypeConvention
    {
        private IPersistenceConfigurer dBConfig;

        public DateColumnConvention(IPersistenceConfigurer dBConfig)
        {
            this.dBConfig = dBConfig;
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => (x.Property.MemberInfo as PropertyInfo).PropertyType == typeof(DateTime)            
                                    || (x.Property.MemberInfo as PropertyInfo).PropertyType == typeof(DateTime?)
            );
        }

        

        public void Apply(IPropertyInstance instance)
        {
            bool isUtc = (instance.Property.MemberInfo as PropertyInfo).IsDefined(typeof(UtcTimeAttribute));
            bool isLocal = (instance.Property.MemberInfo as PropertyInfo).IsDefined(typeof(LocalTimeAttribute));

            if(isUtc && isLocal)
            {
                throw new Exception("Cannot put UtcTimeAttribute and LocalTimeAttribute on same property: " + instance.Property.DeclaringType.FullName + "." + instance.Property.Name);
            }

            DateTimeKind dateTimeKind;
            if (isUtc)
            {
                dateTimeKind = DateTimeKind.Utc;
            }
            else if(isLocal)
            {
                dateTimeKind = DateTimeKind.Local;
            }
            else
            {
                dateTimeKind = DateTimeKind.Unspecified;
            }



            switch (dateTimeKind)
            {
                case DateTimeKind.Utc:
                    instance.CustomType<UtcDateTimeType>();
                    break;
                case DateTimeKind.Local:
                    instance.CustomType<LocalDateTimeType>();
                    break;
                case DateTimeKind.Unspecified:
                default:
                    if (dBConfig is PostgreSQLConfiguration)
                    {
                        instance.CustomType<LocalDateTimeType>();
                    }
                    else
                    {

                        instance.CustomType<DateTimeType>();
                    }
                    break;
            }
        }
    }
}
