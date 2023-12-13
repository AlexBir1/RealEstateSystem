using DwellingAPI.DAL.UOW;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly IDBRepository _dBRepository;

        public RolesService(IDBRepository dBRepository)
        {
            _dBRepository = dBRepository;
        }

        public async Task<ResponseWrapper<IEnumerable<string>>> GetAvailableRoles()
        {
            return new ResponseWrapper<IEnumerable<string>>(newData: await _dBRepository.RolesRepo.GetAvailableRoles());
        }

        public async Task<ResponseWrapper<IEnumerable<string>>> SetAvailableRoles(IEnumerable<string> newRoles)
        {
            return new ResponseWrapper<IEnumerable<string>>(newData: await _dBRepository.RolesRepo.SetAvailableRoles(newRoles));
        }
    }
}
