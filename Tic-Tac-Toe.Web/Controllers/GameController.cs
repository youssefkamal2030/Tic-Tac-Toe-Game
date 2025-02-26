using Microsoft.AspNetCore.Mvc;
using Tic_Tac_Toe.Application.Interfaces;
using Tic_Tac_Toe.Domain.Entities;

namespace Tic_Tac_Toe.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            int? gameId = HttpContext.Session.GetInt32("GameId");
            Game game = gameId.HasValue
                ? await _gameService.GetGameAsync(gameId.Value)
                : await _gameService.StartNewGameAsync();

            if (game == null) game = await _gameService.StartNewGameAsync();
            HttpContext.Session.SetInt32("GameId", game.Id);
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> MakeMove(int row, int col)
        {
            int gameId = HttpContext.Session.GetInt32("GameId") ?? 0;
            var (success, message) = await _gameService.MakeMoveAsync(gameId, row, col);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reset()
        {
            int gameId = HttpContext.Session.GetInt32("GameId") ?? 0;
            await _gameService.ResetGameAsync(gameId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SetMode(string mode)
        {
            int gameId = HttpContext.Session.GetInt32("GameId") ?? 0;
            bool isSinglePlayer = mode == "single";
            await _gameService.SetGameModeAsync(gameId, isSinglePlayer);
            return RedirectToAction("Index");
        }
    }
}
