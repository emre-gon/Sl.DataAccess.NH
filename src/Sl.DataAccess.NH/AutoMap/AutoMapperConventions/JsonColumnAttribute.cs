﻿using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JsonColumnAttribute : Attribute
    {

    }


    public class JsonColumnConvention : IUserTypeConvention
    {
        private IPersistenceConfigurer dBConfig;

        public JsonColumnConvention(IPersistenceConfigurer dBConfig)
        {
            this.dBConfig = dBConfig;
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => (x.Property.MemberInfo as PropertyInfo).IsDefined(typeof(JsonColumnAttribute), false));
        }


        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(string) && dBConfig is PostgreSQLConfiguration)
            {
                instance.CustomSqlType("json"); //json validation only
                return;
            }


            var jsonColumnType = typeof(JsonColumnType<>).MakeGenericType(instance.Property.PropertyType);

            if(dBConfig is PostgreSQLConfiguration)
            {
                instance.CustomSqlType("jsonb");
            }

            var method = instance.GetType().GetMethods().Where(f => f.Name == "CustomType" && f.IsGenericMethod && !f.GetParameters().Any()).Single();

            var customTypeGen = method.MakeGenericMethod(jsonColumnType);

            customTypeGen.Invoke(instance, null);

            //instance.CustomType<JsonColumnType<ExampleJsonModel>>()
        }
    }
}
