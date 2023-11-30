using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; } = DateTime.Today.AddDays(1);
    }
}
