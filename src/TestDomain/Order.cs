using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestDomain
{
    public class Order : TableBaseWithReadAudit
    {
        [Key]
        public virtual int OrderID { get; set; }

        [Required]
        public virtual Person Person { get; set; }

        public virtual DateTime OrderDate { get; set; }

        [JsonColumn]
        public virtual OrderContent OrderContent { get; set; }
    }




    public class OrderContent
    {
        public List<OrderItem> Items { get; set; }

        public decimal Total { get; set; }
    }


    public class OrderItem
    {
        public string ItemName { get; set; }

        public int Count { get; set; }

        public decimal Total { get; set; }
    }
}
