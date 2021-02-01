using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Id.Models
{
    public class UserClaim
    {
        [Key]
        public int ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string Value { get; set; }

        public Nullable<int> Id { get; set; }
        public virtual User User { get; set; }
    }
}