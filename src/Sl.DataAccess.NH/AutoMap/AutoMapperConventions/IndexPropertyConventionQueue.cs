using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.AutoMapperConventions
{
    public static class PropertyInstanceObjExtensions
    {
        public static PropertyInstanceObj<T> GetFirstRunnable<T>(this List<PropertyInstanceObj<T>> Queue) where T : Attribute, IIndexAttribute
        {
            var maxOrder = Queue.Where(f => f.HasRun).Select(f => f.IndexOrder).DefaultIfEmpty(-1).Max();

            return Queue.FirstOrDefault(f => f.CanRun(maxOrder));
        }
    }
    public class PropertyInstanceObj<T> where T : Attribute, IIndexAttribute
    {
        public PropertyInstanceObj(IInspector NHPropertyInspector, 
            string IndexName, 
            int IndexOrder)
        {
            this.NHPropertyInspector = NHPropertyInspector;
            this.IndexName = IndexName;
            this.IndexOrder = IndexOrder;
            this.HasRun = false;
        }

        public IInspector NHPropertyInspector { get; }
        public string IndexName { get; }
        public int IndexOrder { get; }
        public bool HasRun { get; set; }

        public bool IsUnique
        {
            get
            {
                return typeof(T) == typeof(UniqueAttribute);
            }
        }

        public void ForceRun()
        {
            var propInstance = (NHPropertyInspector as IPropertyInstance);
            var fkInstance = (NHPropertyInspector as IManyToOneInstance);


            if (IsUnique)
            {
                if(propInstance != null)
                {
                    propInstance.UniqueKey(this.IndexName);
                }
                else if(fkInstance != null)
                {
                    fkInstance.UniqueKey(this.IndexName);
                }
            }
            else
            {
                if (propInstance != null)
                {
                    propInstance.Index(this.IndexName);
                }
                else if (fkInstance != null)
                {
                    fkInstance.Index(this.IndexName);
                }
            }
            this.HasRun = true;
        }

        public bool CanRun(int LatestRunIndex)
        {
            return this.IndexOrder == LatestRunIndex + 1;
        }
    }

    public class IndexPropertyConventionQueue<T> : Dictionary<string, List<PropertyInstanceObj<T>>> where T : Attribute, IIndexAttribute
    {

        public bool IsUnique
        {
            get
            {
                return typeof(T) == typeof(UniqueAttribute);
            }
        }

        public void Add(IInspector instance)
        {
            #region indices where i am main prop
            IEnumerable<IIndexAttribute> myIndexAttributes;



            FluentNHibernate.Member Property;

            if (instance is IPropertyInstance)
                Property = (instance as IPropertyInstance).Property;
            else if (instance is IManyToOneInstance)
                Property = (instance as IManyToOneInstance).Property;
            else
                throw new Exception("Unknown Property Instance");


            myIndexAttributes = Property.MemberInfo.GetCustomAttributes<T>();


            var myTable = Property.DeclaringType;
            string myTableName = myTable.Name;
            string myPropertyName = Property.Name;

            foreach(var myAttr in myIndexAttributes)
            {
                var myIndexName = myAttr.GetIndexName(myTableName, myPropertyName);

                if (!this.ContainsKey(myIndexName))
                {
                    this[myIndexName] = new List<PropertyInstanceObj<T>>();
                }

                this[myIndexName].Add(new PropertyInstanceObj<T>(instance, myIndexName, 0));
            }
            #endregion


            #region indices where i am secondary prop
            IEnumerable<PropertyInfo> otherProperties = myTable.GetProperties().Where(f => f.IsDefined(typeof(T), false));


            foreach (var otherProp in otherProperties)
            {
                IEnumerable<IIndexAttribute> otherAttributes = otherProp.GetCustomAttributes<T>();

                foreach(var otherAttr in otherAttributes)
                {
                    var partnerColumns = otherAttr.GetPartnerColumns();

                    for(int i = 0; i < partnerColumns.Length; i++)
                    {
                        if(partnerColumns[i] == myPropertyName)
                        {
                            var otherIndexName = otherAttr.GetIndexName(myTableName, otherProp.Name);

                            if (!this.ContainsKey(otherIndexName))
                                this[otherIndexName] = new List<PropertyInstanceObj<T>>();

                            this[otherIndexName].Add(new PropertyInstanceObj<T>(instance, otherIndexName, i + 1));
                        }
                    }
                }
            }
            #endregion




            Run();
        }


        public void Run()
        {
            foreach(var queue in this.Values)
            {
                var firstRunnable = queue.GetFirstRunnable();

                while(firstRunnable != null)
                {
                    firstRunnable.ForceRun();
                    firstRunnable = queue.GetFirstRunnable();
                }
            }
        }
    }
}
