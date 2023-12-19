using DwellingAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class ApartmentPhoto : IEntity
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }

        public Guid ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }
}
