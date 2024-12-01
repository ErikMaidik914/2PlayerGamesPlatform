using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.exceptions
{
    internal class InvalidGameException : Exception
    {
        public InvalidGameException(string message) : base(message)
        {
        }

        public InvalidGameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidGameException()
        {
        }
    }
}
