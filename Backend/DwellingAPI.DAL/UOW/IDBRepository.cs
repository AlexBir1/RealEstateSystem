using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.UOW
{
    public interface IDBRepository
    {
        ICallRepository CallRepo { get; }
        IAccountRepository AccountRepo { get; }
        IApartmentRepository ApartmentRepo { get; }
        IRolesRepository RolesRepo { get; }
        IContactsRepository ContactsRepo { get; }
        IOrderRepository OrderRepo { get; }
        IAgreementRepository AgreementRepo { get; }

        Task<CommitResponse> CommitAsync();
    }
}
