using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tic_Tac_Toe.Domain.Entities;

namespace Tic_Tac_Toe.Application.Interfaces
{
    public interface IGameService
    {
        Task<Game> StartNewGameAsync();
        Task<(bool Success, string Message)> MakeMoveAsync(int gameId, int row, int col);
        Task<Game> GetGameAsync(int gameId);
        Task ResetGameAsync(int gameId);
        Task SetGameModeAsync(int gameId, bool isSinglePlayer);
    }
}
