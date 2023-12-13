using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.NUnit.Exceptions
{
    [Serializable]
    public class TestOperationFailedException : Exception
    {
        public IEnumerable<string>? Errors { get; private set; }
        public TestOperationFailedException(IEnumerable<string> errors)
        {
            Errors = errors;
        }
        public TestOperationFailedException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors;
        }
        public TestOperationFailedException(string message) : base(message)
        {
        }
    }
}
