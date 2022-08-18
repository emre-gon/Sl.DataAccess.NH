using Sl.DataAccess.NH.AutoMap;
using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sl.DataAccess.NH
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IndexAttribute : Attribute, IIndexAttribute
    {
        public string[] PartnerColumns { get; private set; }
        public IndexAttribute(params string[] PartnerColumns)
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

            var name = $"IX_AG_{TableName}_{string.Join("_", ixPropNames)}"
                .Replace("`", "");


            return name;
        }
    }


    public interface IIndexAttribute 
    {
        string[] GetPartnerColumns();
        string GetIndexName(string TableName, string MainPropertyName);
    }
}
