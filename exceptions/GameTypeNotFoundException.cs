using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.exceptions
{
    internal class GameTypeNotFoundException : Exception
    {
        public GameTypeNotFoundException(string message) : base(message)
        {
        }

        public GameTypeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public GameTypeNotFoundException()
        {
        }
    }
}
