using Sl.DataAccess.NH.AutoMap;
using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sl.DataAccess.NH
{
    /// <summary>
    /// with update audit
    /// </summary>
    public abstract class TableBase : TableBaseWithReadAudit
    {
        public virtual int? LastUpdatedBy { get; set; }
        public virtual DateTime? LastUpdatedAt { get; set; }
    }



    public abstract class TableBaseWithReadAudit : ITableBase
    {
        public virtual int? CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

    }


    public abstract class ITableBase
    {
        public virtual Dictionary<string, object> GetIDValues()
        {
            var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)
                                            .Where(f => f.GetCustomAttributes(typeof(KeyAttribute), false).Any());

            Dictionary<string, object> toBeReturned = new Dictionary<string, object>();
            foreach (var prop in props)
            {
                toBeReturned.Add(prop.Name, prop.GetValue(this));
            }

            return toBeReturned;
        }

        public virtual bool IsNew()
        {
            var keyColumns = this.GetType().GetProperties().Where(f => f.IsDefined(typeof(KeyAttribute), true));

            foreach (var kCol in keyColumns)
            {
                var value = kCol.GetValue(this);

                var defaultValue = kCol.PropertyType.IsValueType ? Activator.CreateInstance(kCol.PropertyType) : null;
                if (value == null && defaultValue == null)
                    return true;
                if (value == null || !value.Equals(defaultValue))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            var myType = this.GetType();
            var dnProp = myType.GetProperty("DisplayName");
            if (dnProp == null)
                return $"{myType.Name} | {string.Join(", ", this.GetIDValues().Select(f => f.Key + "=" + f.Value))}";
            else
                return dnProp.GetValue(this)?.ToString();
        }

        #region nHibernate Equals & GetHashCode
        public override bool Equals(object obj)
        {
            var other = obj as TableBaseWithReadAudit;

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var keyProps = this.GetType().GetProperties().
                Where(f => f.IsDefined(typeof(KeyAttribute), true));

            foreach (var prop in keyProps)
            {
                if (prop.GetValue(this) != prop.GetValue(other))
                    return false;
            }

            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = GetType().GetHashCode();
                var keyProps = this.GetType().GetProperties().
                    Where(f => f.IsDefined(typeof(KeyAttribute), true));

                foreach (var prop in keyProps)
                {
                    hash = (hash * 31) ^ prop.GetValue(this).GetHashCode();
                }
                return hash;
            }
        }
        #endregion


        public virtual void CustomVerify()
        {

        }



        #region triggers
        public virtual void BeforeInsert()
        {

        }
        public virtual void AfterInsert()
        {

        }

        public virtual void BeforeUpdate()
        {

        }
        public virtual void AfterUpdate()
        {

        }

        public virtual void BeforeDelete()
        {

        }

        public virtual void AfterDelete()
        {

        }

        #endregion
    }
}
