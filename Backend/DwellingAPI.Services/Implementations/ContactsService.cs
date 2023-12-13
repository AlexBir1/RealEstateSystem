using AutoMapper;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.UOW;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.Interfaces;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Implementations
{
    public class ContactsService : IContactsService
    {
        private readonly IDBRepository _dBRepository;
        private readonly IMapper _mapper;

        public ContactsService(IDBRepository dBRepository, IMapper mapper)
        {
            _dBRepository = dBRepository;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<ContactModel>> DeleteContacts(string contactId)
        {
            var result = await _dBRepository.ContactsRepo.DeleteAsync(contactId);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ContactModel>(_mapper.Map<ContactModel>(result));
        }

        public async Task<ResponseWrapper<IEnumerable<ContactModel>>> GetAllAsync()
        {
            return new ResponseWrapper<IEnumerable<ContactModel>>(
                _mapper.Map<IEnumerable<ContactModel>>(
                    await _dBRepository.ContactsRepo.GetAllAsync()
                    )
                );
        }

        public async Task<ResponseWrapper<IEnumerable<ContactModel>>> GetContacts()
        {
            return new ResponseWrapper<IEnumerable<ContactModel>>(
                _mapper.Map<IEnumerable<ContactModel>>(
                    await _dBRepository.ContactsRepo.GetAllAsync()
                    )
                );
        }

        public async Task<ResponseWrapper<ContactModel>> InsertContact(ContactModel model)
        {
            var result = await _dBRepository.ContactsRepo.InsertAsync(_mapper.Map<Contact>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ContactModel>(_mapper.Map<ContactModel>(result));
        }
    }
}
