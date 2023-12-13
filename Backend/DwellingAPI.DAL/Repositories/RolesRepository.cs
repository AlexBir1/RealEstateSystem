using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Exceptions;
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

        public async Task<IEnumerable<string>> GetAvailableRoles()
        {
            var roles = _roleManager.Roles.ToList();

            if (roles.Count == 0)
                throw new OperationFailedException("No available roles were found");

            return roles.Select(x => x.Name);

        }

        public async Task<IEnumerable<string>> SetAvailableRoles(IEnumerable<string> newRoles)
        {
            foreach (var role in newRoles)
            {
                if (await _roleManager.FindByNameAsync(role) != null)
                    continue;
                await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }

            return newRoles;
        }
    }
}
