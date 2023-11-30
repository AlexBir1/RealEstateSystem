using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.UOW
{
    public class DBRepository : IDBRepository
    {
        private readonly AppDBContext DatabaseContext;

        public ICallRepository CallRepo { get; private set; }

        public IAccountRepository AccountRepo { get; private set; }

        public IApartmentRepository ApartmentRepo { get; private set; }

        public IRolesRepository RolesRepo { get; private set; }

        public IContactsRepository ContactsRepo { get; private set; }

        public IOrderRepository OrderRepo { get; private set; }

        public IAgreementRepository AgreementRepo { get; private set; }

        public DBRepository(AppDBContext databaseContext, UserManager<Account> userManager, SignInManager<Account> signInManager, RoleManager<IdentityRole> roleManager)
        {
            DatabaseContext = databaseContext;
            CallRepo = new CallRepository(databaseContext);
            ApartmentRepo = new ApartmentRepository(databaseContext);
            AccountRepo = new AccountRepository(userManager, signInManager);
            RolesRepo = new RolesRepository(roleManager);
            ContactsRepo = new ContactsRepository(databaseContext);
            OrderRepo = new OrderRepository(databaseContext);
            AgreementRepo = new AgreementRepository(databaseContext);
        }

        public async Task<CommitResponse> CommitAsync()
        {
            try
            {
                int changeCount = await DatabaseContext.SaveChangesAsync();
                return new CommitResponse(changeCount);
            }
            catch(Exception ex)
            {
                var errors = new List<string>() 
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new CommitResponse(errors);
            }
        }
    }
}
