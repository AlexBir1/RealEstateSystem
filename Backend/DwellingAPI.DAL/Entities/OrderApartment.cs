using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class OrderApartment
    {
        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid ApartmentId { get; set; }

        public Apartment Apartment { get; set; }
    }
}
