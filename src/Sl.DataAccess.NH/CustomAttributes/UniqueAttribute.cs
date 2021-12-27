using Sl.DataAccess.NH.AutoMap;
using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sl.DataAccess.NH
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class UniqueAttribute : ValidationAttribute, IIndexAttribute
    {
        public string[] PartnerColumns { get; private set; }
        public UniqueAttribute(params string[] PartnerColumns)
        {
            this.PartnerColumns = PartnerColumns;
        }

        public string[] GetPartnerColumns()
        {
            return PartnerColumns;
        }
        public string GetIndexName(string TableName, string MainPropertyName)
        {
            List<string> ixPropNames = new List<string>() { MainPropertyName };
            ixPropNames.AddRange(PartnerColumns);


            return $"IX_AG_{TableName}_{string.Join("_", ixPropNames)}";
        }
    }
}
