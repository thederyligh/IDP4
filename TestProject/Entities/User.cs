﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject
{
    [Table("Users")]
    public class User
    {
        [Key]
        [MaxLength(50)]       
        public string SubjectId { get; set; }
    
        [MaxLength(100)]
        [Required]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

        public ICollection<ApplicationUserLogin> Logins { get; set; } = new List<ApplicationUserLogin>();


    }
}
