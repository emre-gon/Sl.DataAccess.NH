using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    public class CheckReserverdKeywordsConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            foreach (var keyword in NHibernateAutoMappingConfiguration.Reserved_Keywords)
            {
                if (instance.Property.Name.ToLower() == keyword)
                    throw new AutoMappingException($"You cannot have a column named '{instance.Property.Name}' " +
                        $"in type '{instance.Property.DeclaringType.FullName}'. " +
                        $"'{keyword}' is a reserved keyword in many databases and causes problems.");
            }  
        }
    }
}