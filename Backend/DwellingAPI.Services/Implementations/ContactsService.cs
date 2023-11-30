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
            try
            {
                var result = await _dBRepository.ContactsRepo.DeleteAsync(contactId);

                if(result.Data is null)
                    return new ResponseWrapper<ContactModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ContactModel>(commitResult.Errors);

                return new ResponseWrapper<ContactModel>(_mapper.Map<ContactModel>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ContactModel>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<ContactModel>>> GetContacts()
        {
            try
            {
                var result = await _dBRepository.ContactsRepo.GetAllAsync();

                if(result.Data is null)
                    return new ResponseWrapper<IEnumerable<ContactModel>>(result.Errors);

                return new ResponseWrapper<IEnumerable<ContactModel>>(_mapper.Map<IEnumerable<ContactModel>>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<ContactModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<ContactModel>> InsertContact(ContactModel model)
        {
            try
            {
                var result = await _dBRepository.ContactsRepo.InsertAsync(_mapper.Map<Contact>(model));

                if(result.Data is null)
                    return new ResponseWrapper<ContactModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ContactModel>(commitResult.Errors);

                return new ResponseWrapper<ContactModel>(_mapper.Map<ContactModel>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ContactModel>(errors);
            }
        }
    }
}
