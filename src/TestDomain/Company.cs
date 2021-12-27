using Sl.DataAccess.NH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestDomain
{
    public class Company : TableBase
    {
        [Key]
        public virtual int CompanyID { get; set; }

        [MaxLength(50)]
        [Required]
        [Unique]
        public virtual string Name { get; set; }


        public virtual Address Address { get; set; }


        public virtual AppValidation Validation { get; set; }
    }
}
