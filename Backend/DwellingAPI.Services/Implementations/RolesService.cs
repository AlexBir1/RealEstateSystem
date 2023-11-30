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
            try
            {
                return await _dBRepository.RolesRepo.GetAvailableRoles();
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<string>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<string>>> SetAvailableRoles(IEnumerable<string> newRoles)
        {
            try
            {
                return await _dBRepository.RolesRepo.SetAvailableRoles(newRoles);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<string>>(errors);
            }
        }
    }
}
