
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface ICallService : IService<CallModel>
    {
        Task<ResponseWrapper<CallModel>> InsertAsync(RequestCallModel model);
        Task<ResponseWrapper<IEnumerable<CallModel>>> GetAllAsync();
        Task<ResponseWrapper<CallModel>> DeleteAsync(string id);
    }
}
