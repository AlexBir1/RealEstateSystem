using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Exceptions
{
    [Serializable]
    public class OperationFailedException : Exception
    {
        public IEnumerable<string>? Errors { get; private set; }
        public OperationFailedException(IEnumerable<string> errors)
        {
            Errors = errors;
        }
        public OperationFailedException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors = errors;
        }
        public OperationFailedException(string message) : base(message)
        {
        }
    }
}
