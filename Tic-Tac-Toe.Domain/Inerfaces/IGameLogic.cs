using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tic_Tac_Toe.Domain.Entities;

namespace Tic_Tac_Toe.Domain.Inerfaces
{
    public interface IGameLogic
    {
        bool MakeMove(Game game, int row, int col, out string message);
        bool CheckWinner(Game game, char player);
        bool IsFull(Game game);
        void MakeAIMove(Game game);
        
    }
}
