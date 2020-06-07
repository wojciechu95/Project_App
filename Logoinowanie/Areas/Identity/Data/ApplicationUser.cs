using Logoinowanie.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logoinowanie.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Salt { get; set; }

        public virtual ICollection<PasswdModel> UserId { get; set; }
    }
}
