using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IAgreementService : IService<AgreementModel>
    {
        Task<ResponseWrapper<AgreementModel>> InsertAsync(AgreementModel model);
        Task<ResponseWrapper<AgreementModel>> UpdateAsync(string apartmentId, AgreementModel model);
        Task<ResponseWrapper<AgreementModel>> GetByIdAsync(string apartmentId);
        Task<ResponseWrapper<IEnumerable<AgreementModel>>> GetAllAsync(string accountId = "");
    }
}
