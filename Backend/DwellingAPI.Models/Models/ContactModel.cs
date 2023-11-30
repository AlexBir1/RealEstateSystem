using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class ContactModel
    {
        public string Id { get; set; } = string.Empty;
        public string ContactOptionName { get; set; } = string.Empty;
        public string ContactOptionValue { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }
    }
}
