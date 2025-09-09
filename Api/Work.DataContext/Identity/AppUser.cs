using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.DataContext.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Work.DataContext
{
    public class AppUser : IdentityUser
    {
        public bool IsSuperUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public bool IsDisable { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int IdUser { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
