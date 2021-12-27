using FluentNHibernate;
using FluentNHibernate.Automapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using Sl.DataAccess.NH.AutoMap.CustomMappingSteps;

namespace Sl.DataAccess.NH.AutoMap
{
    public class NHibernateAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public static readonly string[] Reserved_Keywords = { "not", "user", "select", "from", "join", };
        public override bool ShouldMap(Type type)
        {
            var shouldMap = typeof(ITableBase).IsAssignableFrom(type);

            if (shouldMap)
            {
                foreach(var keyword in Reserved_Keywords)
                {
                    if(type.Name.ToLower() == keyword)
                    {
                        throw new AutoMappingException($"You cannot have a table named '{type.Name}'. The name '{keyword}' is a reserved keyword in many databases and causes problems.");
                    }
                }
            }
            return shouldMap;
        }

        public override bool ShouldMap(Member member)
        {
            if (member.IsMethod)
                return false;
            if (member.IsProperty)
            {
                var prop = (PropertyInfo)member.MemberInfo;
                if (prop.GetGetMethod() == null || prop.GetSetMethod() == null)
                    return false;

                if (prop.IsDefined(typeof(DBIgnore), true))
                    return false;


                foreach (var keyword in Reserved_Keywords)
                {
                    if (prop.Name.ToLower() == keyword)
                    {
                        throw new AutoMappingException($"You cannot have a mappable property named '{prop.Name}' in class '{prop.DeclaringType.FullName}'. The name '{keyword}' is a reserved keyword in many databases and causes problems.");
                    }
                }
            }




            return base.ShouldMap(member);
        }

        public override bool IsComponent(Type type)
        {
            return typeof(IComponent).IsAssignableFrom(type);
        }

        public override bool IsId(Member member)
        {
            if (member.IsProperty)
            {
                return ((PropertyInfo)member.MemberInfo).IsDefined(typeof(KeyAttribute));
            }
            return false;
        }

        public override IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder)
        {
            var automappingSteps = base.GetMappingSteps(mapper, conventionFinder).ToList();

            automappingSteps.Insert(0, new CompositeIdentityStep(this));
            automappingSteps.Insert(1, new PrimaryKeyAsForeignKeyStep(this));

            return automappingSteps;
        }
    }
}
