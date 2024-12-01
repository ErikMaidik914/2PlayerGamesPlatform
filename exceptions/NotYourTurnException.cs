using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.exceptions
{
    internal class NotYourTurnException : Exception
    {
        public NotYourTurnException() { }

        public NotYourTurnException(string message): base(message) { }

        public NotYourTurnException(string message, Exception inner) { }
    }
}
