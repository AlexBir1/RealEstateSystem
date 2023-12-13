using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Shared.Interfaces
{
    public interface IAuthorizationModel
    {
        bool KeepAuthorized { get; set; }
    }
}
