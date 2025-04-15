using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public  class UserModel
    {

            [Required, EmailAddress]
            public string Email { get; set; }

           [Required, MinLength(8)]
            public string Password { get; set; }   // Will be hashed later

            [MaxLength(50)]
            public string? FirstName { get; set; } = string.Empty;

            [MaxLength(50)]
            public string? LastName { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
