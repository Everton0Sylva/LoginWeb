using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models
{
    [Table("AuthToken")]
    public class AuthToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        public string TokenType { get; set; }

        [Required]
        public string TokenValue { get; set; }

        [Required]
        public short Expire { get; set; }
    }
}
