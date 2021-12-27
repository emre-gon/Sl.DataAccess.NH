using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.CustomMappingSteps
{
    internal class PrimaryKeyAsForeignKeyStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;

        public PrimaryKeyAsForeignKeyStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            var keyProps = member.DeclaringType.GetProperties()
                .Where(f => f.IsDefined(typeof(KeyAttribute)));

            if (keyProps.Count() != 1)
                return false;

            return member.IsProperty && ((PropertyInfo)member.MemberInfo).IsDefined(typeof(KeyAttribute))
                    && typeof(ITableBase).IsAssignableFrom(member.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            var classMapping = classMap as ClassMapping;
            if (classMapping == null)
                return;

            if (classMapping.Id == null)
            {
                var tempCompositeIdMapping = new CompositeIdMapping { ContainingEntityType = classMap.Type };
                classMapping.Set(x => x.Id, Layer.Defaults, tempCompositeIdMapping);
            }

            if (!(classMapping.Id is CompositeIdMapping))
                return;

            var compositeIdMapping = classMapping.Id as CompositeIdMapping;

            var type = member.PropertyType;

            var key = new KeyManyToOneMapping
            {
                ContainingEntityType = classMap.Type
            };
            key.Set(x => x.Name, Layer.Defaults, member.Name);
            key.Set(x => x.Class, Layer.Defaults, new TypeReference(member.PropertyType));

            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
            key.AddColumn(columnMapping);
            compositeIdMapping.AddKey(key);
        }
    }
}
