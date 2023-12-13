using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IContactsService
    {
        Task<ResponseWrapper<IEnumerable<ContactModel>>> GetContacts();
        Task<ResponseWrapper<ContactModel>> InsertContact(ContactModel model);
        Task<ResponseWrapper<ContactModel>> DeleteContacts(string contactId);
        Task<ResponseWrapper<IEnumerable<ContactModel>>> GetAllAsync();
    }
}
