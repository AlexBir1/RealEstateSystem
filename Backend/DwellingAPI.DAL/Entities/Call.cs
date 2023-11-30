using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Entities
{
    public class Call
    {
        public Guid Id { get; set; }
        public string ToName { get; set; } = string.Empty;
        public string ToPhone { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }
    }
}
