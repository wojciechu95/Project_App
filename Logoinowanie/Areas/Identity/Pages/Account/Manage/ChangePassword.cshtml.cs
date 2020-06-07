using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Logoinowanie.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace Logoinowanie.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Pole {0} jest wymagane")]
            [StringLength(
                20,
                ErrorMessage = "{0} musi mieć długość co najmniej {2} i maksymalnie {1} znaków.",
                MinimumLength = 8)
            ]
            [RegularExpression(
                @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).*$",
                ErrorMessage = "{0} musi zawierać <strong style=\"text-transform:uppercase;\">co najmniej jedną</strong>:<br /> - cyfrę,<br /> -  wielką i małą literę,<br /> - znak inny niż alfanumeryczny."),
            ]
            [DataType(DataType.Password)]
            [Display(Name = "Aktualne hasło")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane")]
            [StringLength(
                20,
                ErrorMessage = "{0} musi mieć długość co najmniej {2} i maksymalnie {1} znaków.",
                MinimumLength = 8)
            ]
            [RegularExpression(
                @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).*$",
                ErrorMessage = "{0} musi zawierać <strong style=\"text-transform:uppercase;\">co najmniej jedną</strong>:<br /> - cyfrę,<br /> -  wielką i małą literę,<br /> - znak inny niż alfanumeryczny."),
            ]
            [DataType(DataType.Password)]
            [Display(Name = "Nowe hasło")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź nowe hasło")]
            [Compare("NewPassword", ErrorMessage = "Nowe hasło i hasło potwierdzające nie zgadzają się.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można załadować użytkownika z ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można załadować użytkownika z ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("Użytkownik z powodzeniem zmienił swoje hasło.");
            StatusMessage = "Twoje hasło zostało zmienione.";

            return RedirectToPage();
        }
    }
}
