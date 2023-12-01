using AutoMapper;
using DwellingAPI.DAL.UOW;
using DwellingAPI.Services.Implementations;
using DwellingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.UOW
{
    public class ServiceRepository : IServiceRepository
    {
        public IApartmentService ApartmentService { get; private set; }

        public IAccountService AccountService { get; private set; }

        public IRolesService RolesService { get; private set; }

        public ICallService CallService { get; private set; }

        public IOrderService OrderService { get; private set; }

        public IContactsService ContactsService { get; private set; }

        public IAgreementService AgreementService { get; private set; }

        public ServiceRepository(IDBRepository DatabaseRepository, IMapper mapper)
        {
            ApartmentService = new ApartmentService(DatabaseRepository, mapper);
            AccountService = new AccountService(DatabaseRepository, mapper);
            CallService = new CallService(DatabaseRepository, mapper);
            RolesService = new RolesService(DatabaseRepository);
            OrderService = new OrderService(DatabaseRepository, mapper);
            ContactsService = new ContactsService(DatabaseRepository, mapper);
            AgreementService = new AgreementService(DatabaseRepository, mapper);
        }
    }
}
