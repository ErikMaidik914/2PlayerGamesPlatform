using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.service
{
    internal interface IGameService
    {
        IGame Play(int nrParameters, object[] parameters);

        IGame GetGame();

        void SetGame(IGame game);
    }
}
