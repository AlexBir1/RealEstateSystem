using DwellingAPI.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class LogInModel : IAuthorizationModel
    {
        public string UserIdentifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool KeepAuthorized { get; set; } = false;
    }
}
