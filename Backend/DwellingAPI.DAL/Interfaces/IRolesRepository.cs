
using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Interfaces
{
    public interface IRolesRepository
    {
        Task<IEnumerable<string>> SetAvailableRoles(IEnumerable<string> newRoles);
        Task<IEnumerable<string>> GetAvailableRoles();
    }
}
