using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestDomain
{
    public class Address : TableBase
    {
        [Key]
        public virtual int AddressID { get; set; }

        [Required]
        [MaxLength(200)]
        public virtual string Description { get; set; }
    }
}
