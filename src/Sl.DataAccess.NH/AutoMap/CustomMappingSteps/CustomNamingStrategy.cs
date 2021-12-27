using NHibernate.Cfg;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sl.DataAccess.NH.AutoMap.CustomMappingSteps
{
    public class CustomNamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className)
        {
            return StringHelper.Unqualify(className);
        }

        public string ColumnName(string columnName)
        {
            return columnName;
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            return (StringHelper.IsNotEmpty(columnName) ? columnName : StringHelper.Unqualify(propertyName));
        }

        public string PropertyToColumnName(string propertyName)
        {
            return StringHelper.Unqualify(propertyName);
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            return StringHelper.Unqualify(propertyName);
        }

        public string TableName(string tableName)
        {
            return tableName;
        }
    }
}
