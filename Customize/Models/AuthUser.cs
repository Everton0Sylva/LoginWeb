
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customize.Models
{
    public class AuthUser : IdentityUser
    {
        [Required]
        public virtual string FullName { get; set; }

        public virtual bool Locked { get; set; }
        public virtual DateTime? LockedDate { get; set; }
        public virtual string provider { get; set; }
        public virtual string providerKey { get; set; }

        [NotMapped]
        public virtual ICollection<AuthRule> Rules { get; }
    }
}
