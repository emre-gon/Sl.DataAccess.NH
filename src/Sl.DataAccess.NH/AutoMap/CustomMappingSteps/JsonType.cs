using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using System.Data.Common;
using NHibernate.Engine;
using Newtonsoft.Json;
using NHibernate.Json;
using System.Runtime.Serialization;
using NpgsqlTypes;
using Npgsql;

namespace Sl.DataAccess.NH.AutoMap.CustomMappingSteps
{
    public class JsonType<T> : IUserType where T : class
    {
        public System.Type ReturnedType => typeof(T);

        public bool IsMutable => false;

        public SqlType[] SqlTypes => new[] { new NpgsqlExtendedSqlType(DbType.Object, NpgsqlDbType.Json) };

        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object DeepCopy(object value)
        {
            return value == null ? null : JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        public new bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }


        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public virtual object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var json = NHibernateUtil.String.NullSafeGet(rs, names[0], session) as string;
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public virtual void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var json = value == null ? null : JsonConvert.SerializeObject(value);
            NHibernateUtil.String.NullSafeSet(cmd, json, index, session);
        }

        public virtual object Replace(object original, object target, object owner)
        {
            return original;
        }
    }
}
