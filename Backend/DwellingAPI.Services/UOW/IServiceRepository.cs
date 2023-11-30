using DwellingAPI.DAL.UOW;
using DwellingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.UOW
{
    public interface IServiceRepository
    {
        IApartmentService ApartmentService { get; }
        IAccountService AccountService { get; }
        IRolesService RolesService { get; }
        ICallService CallService { get; }
        IOrderService OrderService { get; }
        IContactsService ContactsService { get; }
        IAgreementService AgreementService { get; }
    }
}
