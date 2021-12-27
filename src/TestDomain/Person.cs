using Sl.DataAccess.NH;
using Sl.DataAccess.NH.AutoMap.AutoMapperConventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestDomain
{
    public class Person : TableBase
    {
        [Key]
        public virtual int PersonID { get; set; }

        [Index]
        [Required]
        [MaxLength(100)]
        public virtual string NameSurname { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime? BirthDay { get; set; }


        public virtual Address Address { get; set; }

        public virtual AppValidation Validation { get; set; }
             

    }



}
