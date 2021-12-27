using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sl.DataAccess.NH.AutoMap.CustomMappingSteps
{
    internal class CompositeIdentityStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;

        public CompositeIdentityStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            var keyProps = member.DeclaringType.GetProperties()
                .Where(f => f.IsDefined(typeof(KeyAttribute)));

            if (keyProps.Count() < 2)
                return false;

            return member.IsProperty && ((PropertyInfo)member.MemberInfo).IsDefined(typeof(KeyAttribute));
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
            if (type.IsEnum)
                type = typeof(GenericEnumMapper<>).MakeGenericType(type);
         

            if (typeof(ITableBase).IsAssignableFrom(type))
            {
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
            else
            {
                var key = new KeyPropertyMapping
                {
                    ContainingEntityType = classMap.Type
                };
                key.Set(x => x.Name, Layer.Defaults, member.Name);
                key.Set(x => x.Type, Layer.Defaults, new TypeReference(type));
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
                key.AddColumn(columnMapping);
                compositeIdMapping.AddKey(key);
            }

        }
    }
}