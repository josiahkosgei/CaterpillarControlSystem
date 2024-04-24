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
        private int headX;
        private int headY;

        // Tail position
        private int tailX;
        private int tailY;

        // segment &&  List of segment positions
        public List<char> segments = new List<char> { 'H', 'T' };
        private List<(int x, int y)> segmentPosition;

        // store executed commands
        List<string> commandTrail = new List<string>();
        Stack<List<char>> commandsStack = new Stack<List<char>>();

        // to define Land Properties
        public List<(int, int)> spiceLocations = new List<(int, int)>(); // List of spice coordinates
        public List<(int, int)> boosterLocations = new List<(int, int)>(); // List of booster coordinates
        public List<(int, int)> obstacleLocations = new List<(int, int)>(); // List of obstacle coordinates

        // symbols
        private const char HeadSymbol = 'H';
        private const char TailSymbol = 'T';
        private const char BoosterSymbol = 'B';
        private const char SpiceSymbol = '$';
        private const char ObstacleSymbol = '#';

        protected ILogger _logger;

        public Caterpillar()
        {
            Length = 2;


            headX = 0;
            headY = 0;
            tailX = 0;
            tailY = 0;
            segmentPosition = new List<(int x, int y)> { (0, 0), (0, 0) };

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
                    logCommands("Move Up");
                    break;
                case "D":
                    headX += steps;
                    logCommands("Move Down");
                    break;
                case "L":
                    headY -= steps;
                    logCommands("Move Left");
                    break;
                case "R":
                    headY += steps;
                    logCommands("Move Right");
                    break;
            }


            segmentPosition[0] = (headX, headY);

            // Update tail position based on proximity to head
            UpdateTailPosition();
            segmentPosition[1] = (tailX, tailY);

            // Handle interactions with spices, boosters, and obstacles
            HandleInteractions(headX, headY);

            // Ensure caterpillar sensitivity to obstacles
            //CheckObstacleSensitivity();

        }
        public void Grow()
        {
            int distanceY = Math.Abs(headY - tailY);
            if (distanceY < MAX_SEGMENTS)
            {
                segments.Add('-');
                Length++;
                headY++;
                segmentPosition[0] = (headX, headY);
                // Update tail position
                //UpdateTailPosition();

                logCommands("Grow");
                commandsStack.Push(new List<char>(segments));
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


                logCommands("Shrink");
                commandsStack.Push(new List<char>(segments));
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
                var lastCommand = commandTrail[commandTrail.Count - 1];
                // Implement undo logic based on the command
                // ...
                commandTrail.RemoveAt(commandsStack.Count - 1);
            }
            else
            {
                _logger.Information("No commands to undo.");
            }
        }
        public void RedoCommand()
        {
            if (commandsStack.Count > 0)
            {
                var lastCommand = commandTrail[commandTrail.Count - 1];
                // Implement undo logic based on the command
                // ...
                commandTrail.RemoveAt(commandsStack.Count - 1);
            }
            else
            {
                _logger.Information("No commands to undo.");
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
        private void logCommands(string riderCommand)
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
            //Set head and tail positions
            var head = segmentPosition[0];
            var tail = segmentPosition[1];
            landGrid[head.x, head.y] = HeadSymbol; // Head
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
        public int GetLength()
        {
            return Length;
        }
        private void HandleInteractions(int headX, int headY)
        {
            // Handle interactions with spices, boosters, and growth
            // Check for obstacle collision
            if (ObstacleCollision(headX, headY))
            {
                DisintegrateCaterpillar(); // Handle collision
                return; // Exit early
            }

            // Check for spice at the new head position
            if (SpiceCollision(headX, headY))
            {

                segments.Add('$');
                // Log spice collision
                logCommands("SpiceCollision");
                commandsStack.Push(new List<char>(segments));
            }

            // Check for booster collision
            if (BoosterCollision(headX, headY))
            {
                Grow();

                logCommands("BoosterCollision");
                commandsStack.Push(new List<char>(segments));

            }

        }
        private void UpdateTailPosition()
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
                if (headX > tailX)
                    tailX += Math.Min(pullThreshold, headX - tailX); // Move right
                else if (headX < tailX)
                    tailX -= Math.Min(pullThreshold, tailX - headX); // Move left

                if (headY > tailY)
                    tailY += Math.Min(pullThreshold, headY - tailY); // Move down
                else if (headY < tailY)
                    tailY -= Math.Min(pullThreshold, tailY - headY); // Move up
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
