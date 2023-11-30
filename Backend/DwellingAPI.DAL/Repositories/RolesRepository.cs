using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseWrapper<IEnumerable<string>>> GetAvailableRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();

                if(!roles.Any())
                    return new ResponseWrapper<IEnumerable<string>>(new List<string> { new string("No available roles.")});

                return new ResponseWrapper<IEnumerable<string>>(roles.Select(x=>x.Name));
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

        public async Task<ResponseWrapper<IEnumerable<string>>> SetAvailableRoles(IEnumerable<string> newRoles)
        {
            try
            {
                foreach(var role in newRoles)
                {
                    if (await _roleManager.FindByNameAsync(role) != null)
                        continue;
                    await _roleManager.CreateAsync(new IdentityRole { Name = role });
                }
                    
                return new ResponseWrapper<IEnumerable<string>>(newData: newRoles);
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
