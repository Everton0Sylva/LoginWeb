using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Customize.Models
{
    public class AuthToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AuthUser User { get; set; }
        public string TokenType { get; set; }
        public string TokenValue { get; set; }
        public short Expire { get; set; }
    }
}
