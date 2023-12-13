using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IService<T>
    {
        Task<ResponseWrapper<T>> UpdateAsync(string id, T model);
        Task<ResponseWrapper<T>> DeleteAsync(string id);
    }
}
