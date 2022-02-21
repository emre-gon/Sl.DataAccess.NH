using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System.Reflection;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    /// <summary>
    /// This is a convention that will be applied to all entities in your application. What this particular
    /// convention does is to specify that many-to-one, one-to-many, and many-to-many relationships will all
    /// have their Cascade option set to All.
    /// </summary>
    public class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        private void SetCascadeType(ICascadeInstance cascadeInstance, MemberInfo memberInfo)
        {
            var cascadeTypeAtttribute = (memberInfo as PropertyInfo).GetCustomAttribute<CascadeTypeAttribute>();

            CascadeType cascadeType = cascadeTypeAtttribute?.CascadeType ?? CascadeType.None;

            switch (cascadeType)
            {
                case CascadeType.All:
                    cascadeInstance.All();
                    break;
                case CascadeType.SaveUpdate:
                    cascadeInstance.SaveUpdate();
                    break;
                case CascadeType.Merge:
                    cascadeInstance.Merge();
                    break;
                case CascadeType.Delete:
                    cascadeInstance.Delete();
                    break;
                case CascadeType.None:
                default:
                    cascadeInstance.None();
                    break;
            }
        }

        public void Apply(IManyToOneInstance instance)
        {
            SetCascadeType(instance.Cascade, instance.Property.MemberInfo);
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            SetCascadeType(instance.Cascade, instance.Member);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            SetCascadeType(instance.Cascade, instance.Member);
        }
    }
}