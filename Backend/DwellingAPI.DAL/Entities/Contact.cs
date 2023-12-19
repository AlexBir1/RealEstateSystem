using DwellingAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class Contact : IEntity
    {
        public Guid Id { get; set; }
        public string ContactOptionName { get; set; } = string.Empty;
        public string ContactOptionValue { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }
    }
}
