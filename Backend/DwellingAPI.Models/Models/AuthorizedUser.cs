using DwellingAPI.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class AuthorizedUser : IAuthorizationModel
    {
        public string UserId { get; set; } = string.Empty;
        public string JWT { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime TokenExpirationDate { get; set; }
        public bool KeepAuthorized { get; set; } = false;
    }
}
