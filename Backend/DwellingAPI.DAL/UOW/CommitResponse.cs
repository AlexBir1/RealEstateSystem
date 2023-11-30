using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.UOW
{
    public class CommitResponse
    {
        public bool ChangesCommited { get; private set; }
        public int ChangeCount { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        public CommitResponse(IEnumerable<string> errors)
        {
            Errors = errors;
            ChangesCommited = false;
            ChangeCount = 0;
        }

        public CommitResponse(int changeCount)
        {
            ChangesCommited = changeCount > 0;
            ChangeCount = changeCount;
            Errors = new List<string>();
        }
    }
}
