using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class ApartmentDetails
    {
        public Guid ApartmentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string RealtorName { get; set; } = string.Empty;
        public string RealtorPhone { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }

        public virtual Apartment? Apartment { get; set; }
    }
}
