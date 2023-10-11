using GameOfLife.Models;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService; 
        }

        [HttpGet("getNextGeneration")]
        public async Task<IEnumerable<Coordinates>> GetNextGeneration()
        {
            var nextGeneration = await _gameService.StartGame();

            //Note: set the processed generation into current generation to be processed again.
            await _gameService.InitializePattern(nextGeneration);

            return nextGeneration;
        }

        [HttpGet("getConfig")]
        public async Task<GridConfig> GetGridSize()
        {
            return await _gameService.GetGridSize();
        }

        [HttpPost("stop")]
        public async void StopGame()
        {
            await _gameService.ResetPattern();
        }

        [HttpPost("init")]
        public async void InitializePattern([FromBody] List<Coordinates> pattern)
        {
            await _gameService.InitializePattern(pattern);
        }
    }
}
