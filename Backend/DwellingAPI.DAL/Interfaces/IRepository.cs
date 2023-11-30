using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Interfaces
{
    public interface IRepository<T>
    {
        Task<ResponseWrapper<T>> InsertAsync(T entity);
        Task<ResponseWrapper<T>> UpdateAsync(string id, T entity);
        Task<ResponseWrapper<T>> DeleteAsync(string id);
        Task<ResponseWrapper<IEnumerable<T>>> GetAllAsync();
        Task<ResponseWrapper<T>> GetByIdAsync(string id);
    }
}
