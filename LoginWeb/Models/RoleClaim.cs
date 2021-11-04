using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWeb.Models
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public int ClaimId { get; set; }
        [ForeignKey("ClaimId")]
        public AuthClaim Claim { get; set; }
    }
}
