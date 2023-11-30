using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class ChangePasswordModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "New password do not match its confirmation")]
        public string NewPasswordConfirm { get; set; } = string.Empty;
    }
}
