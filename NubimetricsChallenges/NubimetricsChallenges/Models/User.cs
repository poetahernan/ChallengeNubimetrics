using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NubimetricsChallenge01Countries.Models
{
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public string email { get; set; }

        public string password { get; set; }

    }
}
