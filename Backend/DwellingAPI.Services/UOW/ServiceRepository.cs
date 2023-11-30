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
            DBRepository db = (DBRepository)DatabaseRepository;
            ApartmentService = new ApartmentService(db, mapper);
            AccountService = new AccountService(db, mapper);
            CallService = new CallService(db, mapper);
            RolesService = new RolesService(db);
            OrderService = new OrderService(db, mapper);
            ContactsService = new ContactsService(db, mapper);
            AgreementService = new AgreementService(db, mapper);
        }
    }
}
