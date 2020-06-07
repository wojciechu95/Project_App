using Logoinowanie.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Logoinowanie.Models
{
    public class PasswdModel
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [EmailAddress(ErrorMessage = "To nie jest email. Proszę adres email.")]
        [Display(Name = "Email, na który zarejestrowano")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Adres url strony")]
        [DataType(DataType.Url)]
        public string UrlP { get; set; }

        [Required]
        [Display(Name = "Login do strony")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Hasło do strony")]
        public string Passwd { get; set; }
        
        [Display(Name = "Opis danych w portfelu")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public string SaltKey { get; set; }

        public string KeyIV { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}