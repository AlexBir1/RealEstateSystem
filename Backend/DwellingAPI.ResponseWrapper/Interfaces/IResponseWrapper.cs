using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.ResponseWrapper.Interfaces
{
    public interface IResponseWrapper<T>
    {
        T? Data { get; }
        IEnumerable<string> Errors { get; }
    }
}
