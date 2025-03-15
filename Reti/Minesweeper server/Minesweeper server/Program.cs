using System.Net.Http.Headers;

namespace Minesweeper_server
{
    public class Game
    {
        public enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }

        private byte[,] Matrix;
        private bool[,] IsOpened;
        private List<string> OpenedCells = new List<string>();
        private Random rnd = new Random();
        private List<int> BombCoordinates;
        public Difficulty difficulty { get; private set; }
        public int BombsNumber { get; private set; }

        public Game(int dimensions, Difficulty difficulty)
        {
            Matrix = new byte[dimensions, dimensions];
            IsOpened = new bool[dimensions, dimensions];
            this.difficulty = difficulty;

            Setup();
        }

        private void Setup()
        {
            // Set BombsNumber based on difficulty
            switch (difficulty)
            {
                case Difficulty.Easy:
                    BombsNumber = 10;
                    break;
                case Difficulty.Normal:
                    BombsNumber = 20;
                    break;
                case Difficulty.Hard:
                    BombsNumber = 30;
                    break;
                default:
                    throw new ArgumentException("Invalid difficulty");
            }

            // Generate bomb coordinates
            BombCoordinates = new List<int>(BombsNumber);
            int totalCells = Matrix.GetLength(0) * Matrix.GetLength(1);
            int generated_bombs = BombsNumber;
            while (generated_bombs > 0)
            {
                int curr = rnd.Next(0, totalCells);
                if (BombCoordinates.Contains(curr))
                    continue;
                BombCoordinates.Add(curr);
                generated_bombs--;
            }
        }

        // Counts bombs adjacent to (x, y)
        private int CountAdjacentBombs(int x, int y)
        {
            int count = 0;
            int rows = Matrix.GetLength(0);
            int cols = Matrix.GetLength(1);
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    int newX = x + dx;
                    int newY = y + dy;
                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                    {
                        int coordinate = newX * rows + newY;
                        if (BombCoordinates.Contains(coordinate))
                            count++;
                    }
                }
            }
            return count;
        }

        // Recursively open adjacent cells if there are no bombs around
        private void OpenAdjacentCells(int x, int y)
        {
            int rows = Matrix.GetLength(0);
            int cols = Matrix.GetLength(1);
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    int newX = x + dx;
                    int newY = y + dy;
                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                    {
                        if (!IsOpened[newX, newY])
                        {
                            Reveal(newX, newY);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reveals a cell at (x, y): counts adjacent bombs, updates the matrix, records the opened cell in the list, and recursively opens neighbors if needed.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if the game can continue, false otherwise</returns>
        private bool Reveal(int x, int y)
        {
            if (IsOpened[x, y])
                return true;

            int rows = Matrix.GetLength(0);
            int coordinate = x * rows + y;

            // If it's a bomb, You lose
            if (BombCoordinates.Contains(coordinate))
            {
                return false;
            }

            int adjacentBombs = CountAdjacentBombs(x, y);
            Matrix[x, y] = (byte)adjacentBombs;
            IsOpened[x, y] = true;
            OpenedCells.Add($"({x},{y}):{adjacentBombs}");

            // If there are no bombs adjacent, recursively reveal neighbors.
            if (adjacentBombs == 0)
            {
                OpenAdjacentCells(x, y);
            }

            return true;
        }

        private bool CheckWin()
        {
            // Checks if there are any not opened cells
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (!IsOpened[i, j]) // if the cell that is not open contains a bomb, it's ok and the check continues
                    {
                        bool isBomb = false;
                        foreach (int coordinate in BombCoordinates)
                            if(i * Matrix.GetLength(0) + j == coordinate)
                            {
                                isBomb = true;
                                break;
                            }

                        if(!isBomb) return false;
                    }
                }
            }

            // if all the cells are open (except for the bomb cells) the user wins
            return true;
        }

        public bool Open(int x, int y)
        {
            if (x < 0 || x >= Matrix.GetLength(0))
                throw new ArgumentException("Invalid X");
            if (y < 0 || y >= Matrix.GetLength(1))
                throw new ArgumentException("Invalid Y");

            OpenedCells.Clear();
            bool result = Reveal(x, y);

            if(result) CheckWin(); // If the user can still play, checks if he won

            return result;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
