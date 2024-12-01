using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.exceptions
{
    internal class InvalidDifficultyException : Exception
    {
        public InvalidDifficultyException(string message) : base(message)
        {
        }

        public InvalidDifficultyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidDifficultyException()
        {
        }
    }
}
