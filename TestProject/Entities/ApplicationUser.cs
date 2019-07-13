using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestProject
{
    [Table("USERS")]
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [MaxLength(50)]
        [Required]
        public string Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public ICollection<ApplicationClaim> Claims { get; set; } = new List<ApplicationClaim>();
        public ICollection<ApplicationUserLogin> Logins { get; set; } = new List<ApplicationUserLogin>();

    }
}
