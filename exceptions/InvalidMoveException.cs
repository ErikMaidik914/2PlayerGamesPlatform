using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.exceptions
{
    internal class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message) : base(message)
        {
        }
        public InvalidMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidMoveException()
        {
        }
    }
}
