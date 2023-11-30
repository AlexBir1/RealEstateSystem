using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Models
{
    public class ApartmentPhotoModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public Guid ApartmentId { get; set; }
        public IFormFile PhotoFile { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }
    }
}
