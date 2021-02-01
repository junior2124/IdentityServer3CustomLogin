using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Id.Models
{
    public class User
    {
        public User()
        {
            this.UserClaims = new HashSet<UserClaim>();
        }

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool AcceptedEula { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; set; }
    }
}