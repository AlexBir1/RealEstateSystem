using Bogus.DataSets;
using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.NUnit.ResponseWrapper
{
    public class TestResponseWrapper<T>
    {
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}
