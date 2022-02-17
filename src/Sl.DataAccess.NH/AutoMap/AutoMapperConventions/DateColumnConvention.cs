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
            criteria.Expect(x => (x.Property.MemberInfo as PropertyInfo).PropertyType == typeof(DateTime));
        }


        public void Apply(IPropertyInstance instance)
        {
            bool isUtc = (instance.Property.MemberInfo as PropertyInfo).IsDefined(typeof(UtcTimeAttribute));

            if (isUtc)
            {
                instance.CustomType<UtcDateTimeType>();
            }
            else
            {
                instance.CustomType<LocalDateTimeType>();
            }
        }
    }
}
