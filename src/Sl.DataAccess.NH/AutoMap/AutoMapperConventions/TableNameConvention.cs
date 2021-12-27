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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public TableNameAttribute(string Name)
        {
            this.Name = Name;
        }
    }



    public class TableNameConvention : IClassConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
        {
            if (instance.EntityType.IsDefined(typeof(TableNameAttribute), false))
            {
                string name = instance.EntityType.GetCustomAttribute<TableNameAttribute>(false).Name;

                instance.Table(name);
            }
            else
            {
                instance.Table(instance.TableName);                
            }
        }
    }
}