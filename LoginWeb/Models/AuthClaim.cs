using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWeb.Models
{
    [Table("AuthClaim")]
    public class AuthClaim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Rule { get; set; }

        [Required]
        public ClaimType Type { get; set; }
    }

    public enum ClaimType
    {
        Writer,
        Read,
        Delete
    };

}