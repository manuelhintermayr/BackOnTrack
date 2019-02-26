using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.Infrastructure.Exceptions
{
    public class NewEntryException : Exception
    {
        public NewEntryException(string message) : base(message) { }
    }
}
