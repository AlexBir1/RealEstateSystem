using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class Account : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Agreement> Agreements { get; set; }
    }
}
