using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tic_Tac_Toe.Application.Interfaces;
using Tic_Tac_Toe.Domain.Entities;
using Tic_Tac_Toe.Domain.Inerfaces;

namespace Tic_Tac_Toe.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameLogic _gameLogic;

        public GameService(IGameRepository gameRepository, IGameLogic gameLogic)
        {
            _gameRepository = gameRepository;
            _gameLogic = gameLogic;
        }

        public async Task<Game> StartNewGameAsync()
        {
            var game = new Game();
            await _gameRepository.AddAsync(game);
            return game;
        }

        public async Task<Game> GetGameAsync(int gameId)
        {
            return await _gameRepository.GetByIdAsync(gameId);
        }

        public async Task ResetGameAsync(int gameId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game != null)
            {
                game.Board = "         ";
                game.CurrentPlayer = 'X';
                game.Status = "InProgress";
                await _gameRepository.UpdateAsync(game);
            }
        }
        public async Task SetGameModeAsync(int gameId, bool isSinglePlayer)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game != null)
            {
                game.IsSinglePlayer = isSinglePlayer;
                game.Board = "         ";
                game.CurrentPlayer = 'X';
                game.Status = "InProgress";
                await _gameRepository.UpdateAsync(game);
            }
        }
        public async Task<(bool Success, string Message)> MakeMoveAsync(int gameId, int row, int col)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
                return (false, "Game not found.");

            var (success, message) = (false, "");
            if (_gameLogic.MakeMove(game, row, col, out message))
            {
                if (game.IsSinglePlayer && game.CurrentPlayer == 'O' && game.Status == "InProgress")
                {
                    _gameLogic.MakeAIMove(game);
                }
                await _gameRepository.UpdateAsync(game);
                success = true;
            }
            return (success, message);
        }
       
    }
}
