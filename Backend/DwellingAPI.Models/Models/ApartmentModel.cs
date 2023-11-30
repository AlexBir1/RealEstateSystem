using DwellingAPI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class ApartmentModel
    {
        public string Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RealtorName { get; set; } = string.Empty;
        public string RealtorPhone { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public int Rooms { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        [AllowNull]
        public virtual ICollection<OrderModel> Orders { get; set; }
        [AllowNull]
        public virtual ICollection<ApartmentPhotoModel> Photos { get; set; }
    }
}
