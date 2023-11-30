using DwellingAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class AgreementModel
    {
        public string Id { get; set; }

        public string AccountId { get; set; } = string.Empty;

        public decimal SumPerMonth { get; set; }
        public decimal RealtorPaymentSum { get; set; }

        public string ApartmentNumber { get; set; } = string.Empty;
        public string ApartmentCity { get; set; } = string.Empty;
        public string ApartmentAddress { get; set; } = string.Empty;

        public int PaymentsMadeCount { get; set; }
        public int PaymentsToMakeCount { get; set; }

        public int MonthCountBeforeExpiration { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastlyUpdatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
