using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.exceptions
{
    internal class InvalidColumnException : Exception
    {

        public InvalidColumnException(string message) : base(message)
        {
        }

        public InvalidColumnException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidColumnException()
        {
        }
    }
}
