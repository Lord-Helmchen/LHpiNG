using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNg.Web
{
    [Serializable]
    class ScrapingException : Exception
    {
        public ScrapingException()
        {
        }

        public ScrapingException(string message) : base(message)
        {
        }

        public ScrapingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
