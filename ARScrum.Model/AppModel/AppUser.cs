using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Model.AppModel
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(200)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string? LastName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}
