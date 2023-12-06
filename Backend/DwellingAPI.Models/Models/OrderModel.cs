using DwellingAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class OrderModel
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string City { get; set; }
        public int EstimatedRoomsQuantity { get; set; }
        public decimal EstimatedPriceLimit { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }

        public ICollection<ApartmentModel> Apartments { get; set; }
    }
}
