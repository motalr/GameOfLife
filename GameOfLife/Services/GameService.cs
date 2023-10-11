using GameOfLife.Models;

namespace GameOfLife.Services
{
    public class GameService : IGameService
    {
        private List<Coordinates> _nextGeneration { get; set; }
        private List<Coordinates> _currentGeneration { get; set; }

        private int totalRows = 0; // Number of rows in the grid
        private int totalColumns = 10; // Number of columns in the grid

        public GameService()
        {
            _nextGeneration = new List<Coordinates>();
            _currentGeneration = new List<Coordinates>();
        }

        public Task InitializePattern(List<Coordinates> coordinates)
        {
            _currentGeneration = coordinates;

            return Task.CompletedTask;
        }

        static int CountLiveNeighbors(bool[,] grid, int row, int col)
        {
            int liveNeighbors = 0;
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = row + dx[i];
                int newCol = col + dy[i];

                if (newRow >= 0 && newRow < grid.GetLength(0) && newCol >= 0 && newCol < grid.GetLength(1))
                {
                    if (grid[newRow, newCol])
                    {
                        liveNeighbors++;
                    }
                }
            }

            return liveNeighbors;
        }

        public Task ResetPattern()
        {
            _currentGeneration = new List<Coordinates>();

            return Task.CompletedTask;
        }

        public Task<List<Coordinates>> StartGame()
        {
            bool[,] currentGrid = new bool[totalRows, totalColumns];
            bool[,] nextGrid = new bool[totalRows, totalColumns];

            _nextGeneration = new List<Coordinates>();

            // Set the cells in the current grid based on the input coordinates
            foreach (var coordinate in _currentGeneration)
            {
                currentGrid[coordinate.Row, coordinate.Col] = true;
            }

            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < totalColumns; col++)
                {
                    int liveNeighbors = CountLiveNeighbors(currentGrid, row, col);

                    if (currentGrid[row, col])
                    {
                        // Any live cell with fewer than two live neighbors dies
                        // Any live cell with two or three live neighbors lives on
                        // Any live cell with more than three live neighbors dies
                        if (liveNeighbors == 2 || liveNeighbors == 3)
                        {
                            _nextGeneration.Add(new Coordinates()
                            {
                                Row = row,
                                Col = col,
                            });
                        }
                    }
                    else
                    {
                        // Any dead cell with exactly three live neighbors becomes a live cell
                        if (liveNeighbors == 3)
                        {
                            _nextGeneration.Add(new Coordinates()
                            {
                                Row = row,
                                Col = col,
                            });
                        }
                    }
                }
            }

            return Task.FromResult(_nextGeneration);
        }

        public Task<GridConfig> GetGridSize()
        {
            var conf = new GridConfig();

            totalRows = conf.TotalRows;
            totalColumns = conf.TotalColumns;

            return Task.FromResult(conf);
        }

    }
}
