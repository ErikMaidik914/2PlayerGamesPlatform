using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.service
{
    internal interface IPlayService
    {
        void Play(int nrParameters, object[] parameters);
        bool IsGameOver();

        List<IPiece> GetBoard();

        Guid GetTurn();
        void PlayOther();
        Guid? GetWinner();

        Guid StartPlayer();

    }
}
