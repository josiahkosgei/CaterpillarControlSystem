using Serilog;

namespace CaterpillarControlSystem.App
{
    public class Caterpillar
    {
        // properties
        private const int GRID_SIZE = 30;
        private const int MAX_SEGMENTS = 5;
        public int Length { get; private set; }

        //Head position
        public int headX;
        public int headY;

        // Tail position
        public int tailX;
        public int tailY;

        // segment &&  List of segment positions
        public List<char> segments = ['H', 'T'];
        public List<(int x, int y)> segmentPosition;

        // store executed commands
        readonly List<string> commandTrail = [];
        readonly Stack<List<(int x, int y)>> commandsStack = new();
        private Stack<List<(int x, int y)>> undoStack = new();


        // to define Land Properties
        public List<(int, int)> spiceLocations = []; // List of spice coordinates
        public List<(int, int)> boosterLocations = []; // List of booster coordinates
        public List<(int, int)> obstacleLocations = []; // List of obstacle coordinates

        // symbols
        private const char HeadSymbol = 'H';
        private const char TailSymbol = 'T';

        protected ILogger _logger;

        public Caterpillar()
        {
            Length = 2;

            headX = 0;
            headY = 0;
            tailX = 0;
            tailY = 0;
            segmentPosition = [(0, 0), (0, 0)];

            _logger = Log.ForContext(GetType());
        }

        // Capterpillar command functions

        public void Move(string direction, int steps)
        {
            // Update head and tail positions
            switch (direction)
            {
                case "U":
                    headX -= steps;
                    LogCommands("Move Up");
                    break;
                case "D":
                    headX += steps;
                    LogCommands("Move Down");
                    break;
                case "L":
                    headY -= steps;
                    LogCommands("Move Left");
                    break;
                case "R":
                    headY += steps;
                    LogCommands("Move Right");
                    break;
                case "Z":
                    UndoCommand();
                    break;
                case "Y":
                    RedoCommand();
                    break;
            }


            segmentPosition[0] = (headX, headY);

            // Update tail position based on proximity to head
            CalculateTailPosition();
            segmentPosition[1] = (tailX, tailY);

            // Handle interactions with spices, boosters, and obstacles
            HandleHeadInteractions(headX, headY);

        }
        public void Grow()
        {
            int distanceY = Math.Abs(headY - tailY);
            if (distanceY < MAX_SEGMENTS)
            {
                headY++;
                segmentPosition[0] = (headX, headY);

                LogCommands("Grow");
                commandsStack.Push(new List<(int x, int y)>(segmentPosition));
            }
            else
            {
                _logger.Error("Cannot grow beyond 5 segments.");
            }
        }
        public void Shrink()
        {

            int distanceY = Math.Abs(headY - tailY);
            if (distanceY > 2)
            {
                segments.RemoveAt(segments.Count - 1);
                Length--;

                headY--;
                segmentPosition[0] = (headX, headY);


                LogCommands("Shrink");
                commandsStack.Push(new List<(int x, int y)>(segmentPosition));
            }
            else
            {
                _logger.Error("Cannot shrink below 2 segments.");
            }
        }
        public void UndoCommand()
        {
            if (commandsStack.Count > 0)
            {
                commandTrail.RemoveAt(commandsStack.Count - 1);
                var prevCommand = commandsStack.Pop();
                undoStack.Push(prevCommand);
            }
            else
            {
                _logger.Information("No commands to undo.");
            }
        }
        public void RedoCommand()
        {
            if (undoStack.Count > 0)
            {
                var nextCommand = undoStack.Pop();
                commandsStack.Push(nextCommand);
            }
            else
            {
                _logger.Information("No commands to redo.");
            }
        }
        public void ExecuteRiderCommand(char riderCommand, int steps = 0)
        {
            switch (riderCommand)
            {
                case 'U':
                case 'D':
                case 'L':
                case 'R':
                    Move(riderCommand.ToString(), steps);
                    break;
                case 'G':
                    Grow();
                    break;
                case 'S':
                    Shrink();
                    break;
                default:
                    _logger.Error("Invalid command. Use U, D, L, R, G, or S.");
                    throw new ArgumentException("Invalid command. Use U, D, L, R, G, or S.");
            }

            // store the executed command
            // Log the valid executed command
            if (riderCommand != 'Z' && riderCommand != 'Y')
            {
                commandTrail.Add(riderCommand.ToString());
            }
        }


        // Helper Functions

        public bool SpiceCollision(int row, int col)
        {
            return spiceLocations.Contains((row, col));
        }
        public bool BoosterCollision(int row, int col)
        {
            return boosterLocations.Contains((row, col));
        }
        public bool ObstacleCollision(int row, int col)
        {
            // Check if the given position collides with any obstacle
            return obstacleLocations.Contains((row, col));
        }

        private void DisintegrateCaterpillar()
        {

            // Remove all segments (caterpillar disintegrates)
            segments.Clear();

            // Update head and tail indices
            headX = -1;
            headY = -1;
            tailX = -1;
            tailY = -1;

            // Log the collision for analysis
            _logger.Error("Obstacle detected! Caterpillar disintegrated.");

        }
        private void LogCommands(string riderCommand)
        {
            // Log command
            _logger.Information(riderCommand);
        }
        public void DisplayRadarImageGrid()
        {

            char[,] landGrid = new char[GRID_SIZE, GRID_SIZE];

            // Initialize the grid with Empty 1x1 square meter of land represented by *
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    landGrid[i, j] = '*';
                }
            }

            // Mark head and tail positions
            var (x, y) = segmentPosition[0];
            var tail = segmentPosition[1];
            landGrid[x, y] = HeadSymbol; // Head
            landGrid[tail.x, tail.y] = TailSymbol;


            // Display the grid
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    Console.Write(landGrid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private void HandleHeadInteractions(int headX, int headY)
        {
            // Handle interactions with spices, boosters, and growth
            // Check for obstacle collision
            if (ObstacleCollision(headX, headY))
            {
                LogCommands("ObstacleCollision");
                DisintegrateCaterpillar(); // Handle collision
                return;
            }

            // Check for spice at the new head position
            if (SpiceCollision(headX, headY))
            {

                segments.Add('$');
                // Log spice collision
                LogCommands("SpiceCollision");
                commandsStack.Push(new List<(int x, int y)>(segmentPosition));
            }

            // Check for booster collision
            if (BoosterCollision(headX, headY))
            {
                Grow();

                LogCommands("BoosterCollision");
                commandsStack.Push(new List<(int x, int y)>(segmentPosition));

            }

        }
        private void CalculateTailPosition()
        {
            // Calculate distance between head and tail
            int distanceX = Math.Abs(headX - tailX);
            int distanceY = Math.Abs(headY - tailY);

            // Define a threshold for pulling the tail (e.g., 5 steps)
            int pullThreshold = 2;

            // Pull the tail toward the head if needed
            if (distanceX > pullThreshold || distanceY > pullThreshold)
            {
                // Adjust tail position based on head movement
                // Move tail toward the head
                if (distanceX > distanceY)
                    tailX = headX > tailX ? tailX + 1 : tailX - 1;
                else
                    tailY = headY > tailY ? tailY + 1 : tailY - 1;
            }
            else if (distanceX > 0 && distanceY > 0)
            {
                // Move tail diagonally toward the head
                tailX = headX > tailX ? tailX + 1 : tailX - 1;
                tailY = headY > tailY ? tailY + 1 : tailY - 1;
            }
        }
    }
}
