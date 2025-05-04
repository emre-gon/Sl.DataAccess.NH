using NHibernate.SqlTypes;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.CustomMappingSteps
{
    [Serializable]
    public class NpgsqlExtendedSqlType : SqlType
    {
        private readonly NpgsqlDbType _npgDbType;

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType) : base(dbType)
        {
            _npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length) : base(dbType, length)
        {
            _npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale) : base(dbType,
            precision, scale)
        {
            _npgDbType = npgDbType;
        }

        public NpgsqlDbType NpgDbType
        {
            get { return _npgDbType; }
        }
    }
}
