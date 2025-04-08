using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Minesweeper_server
{
    public class Game
    {
        const int DIMENSIONS = 10;
        public enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }

        private byte[,] Matrix;
        private bool[,] IsOpened;
        public List<ServerPosition> OpenedCells = new List<ServerPosition>();
        private Random rnd = new Random();
        private List<int> BombCoordinates;
        public Difficulty difficulty { get; private set; }
        public int BombsNumber { get; private set; }

        public Game(Difficulty difficulty)
        {
            Matrix = new byte[DIMENSIONS, DIMENSIONS];
            IsOpened = new bool[DIMENSIONS, DIMENSIONS];
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
            OpenedCells.Add(new ServerPosition(x, y, adjacentBombs));

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

        public (bool HasLost, bool HasWon) Open(int x, int y)
        {
            if (x < 0 || x >= Matrix.GetLength(0))
                throw new ArgumentException("Invalid X");
            if (y < 0 || y >= Matrix.GetLength(1))
                throw new ArgumentException("Invalid Y");

            OpenedCells.Clear();
            bool lost = !Reveal(x, y);
            bool hasWon = false;

            if(!lost) 
                hasWon = CheckWin(); // If the user can still play, checks if he won

            return (lost, hasWon);
        }
    }

    struct ClientGameData
    {
        public ClientPosition Move { get; set; }

    }

    public struct ClientPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public struct ServerPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Adjacent { get; set; }

        public ServerPosition(int x, int y, int adjacent)
        {
            X = x;
            Y = y;
            Adjacent = adjacent;
        }
    }

    public struct ServerGameData
    {
        public bool HasWon { get; set; }
        public bool HasLost { get; set; }
        public List<ServerPosition> OpenedCells { get; set; }

        public ServerGameData(bool hasWon, bool hasLost, List<ServerPosition> openedCells)
        {
            HasWon = hasWon;
            HasLost = hasLost;
            OpenedCells = openedCells;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void game(object handler_obj)
        {
            Socket handler = (Socket)handler_obj;

            Game instance = new Game(Game.Difficulty.Normal);

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[4096];
                    int bytesReceived = handler.Receive(buffer);
                    string json_data = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

                    // ottieni i dati dal client
                    ClientGameData data = JsonSerializer.Deserialize<ClientGameData>(json_data);
                    Console.WriteLine($"Received move: {data.Move.X}, {data.Move.Y}");

                    // gestisci mossa
                    (bool hasLost, bool hasWon) = instance.Open(data.Move.X, data.Move.Y);

                    // manda risultato al client
                    string json_response = JsonSerializer.Serialize(new ServerGameData(hasWon, hasLost, instance.OpenedCells));
                    Console.WriteLine(json_response);
                    byte[] response = Encoding.ASCII.GetBytes(json_response);

                    handler.Send(response);
                    if (hasWon | hasLost)
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Client has disconnected.");
                    break;
                }
                
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            Console.WriteLine("Game ended, connection closed.");
        }

        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ip, 11000);

            try
            {
                Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);

                listener.Listen(10);

                Console.WriteLine("Server is up and listening, use CTRL+C to stop.");

                while (true)
                {
                    Socket handler = listener.Accept();
                    Console.WriteLine("Client connected.");
                    Thread gameInstance = new Thread(game);
                    gameInstance.Start(handler);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }
    }
}
