using GameOfLife.Models;

namespace GameOfLife.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Set cells in current generation
        /// </summary>
        public Task InitializePattern(List<Coordinates> coordinates);

        public Task ResetPattern();

        /// <summary>
        /// process current generation and creating new generation
        /// </summary>
        public Task<List<Coordinates>> StartGame();

        /// <summary>
        /// get total rows and columns
        /// </summary>
        public Task<GridConfig> GetGridSize();

    }
}
