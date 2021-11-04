using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginWeb.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public int ClaimId { get; set; }
        [ForeignKey("ClaimId")]
        public AuthClaim Claim { get; set; }
    }
}
