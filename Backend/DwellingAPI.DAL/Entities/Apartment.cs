using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DwellingAPI.DAL.Entities
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string ImageUrl { get; set; } = string.Empty;
        public int Rooms { get; set; }

        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public virtual ApartmentDetails? Details { get; set; } = new ApartmentDetails();
        public virtual ICollection<ApartmentPhoto> Photos { get; set; }
        public ICollection<OrderApartment> ApartmentOrders { get; set; }
    }
}
