using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DwellingAPI.ResponseWrapper.Interfaces;

namespace DwellingAPI.ResponseWrapper.Implementation
{
    public class ResponseWrapper<T> : IResponseWrapper<T>
    {
        public T? Data { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        public ResponseWrapper(T newData)
        {
            Data = newData;
            Errors = new List<string>();
        }


        public ResponseWrapper(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        
    }
}
