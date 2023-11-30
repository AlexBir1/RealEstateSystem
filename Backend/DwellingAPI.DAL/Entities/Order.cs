using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int EstimatedRoomsQuantity { get; set; }
        public decimal EstimatedPriceLimit { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }

        public Account Account { get; set; }
        public ICollection<OrderApartment> OrderApartments { get; set; }
    }

    public enum OrderStatus
    {
        InProcess,
        SearchForApartment,
        FoundApartment,
        Completed,
        Canceled
    }
}
