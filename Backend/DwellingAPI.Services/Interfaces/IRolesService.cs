using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IRolesService
    {
        Task<ResponseWrapper<IEnumerable<string>>> SetAvailableRoles(IEnumerable<string> newRoles);
        Task<ResponseWrapper<IEnumerable<string>>> GetAvailableRoles();
    }
}
