using Serilog;
using System.Reflection.Metadata.Ecma335;

namespace CaterpillarControlSystem.App
{
    public class Caterpillar
    {
        // Caterpillar properties
        const int GRID_SIZE = 30;
        public int Length { get; private set; }
        public int headIndex = 0;

        public List<char> segments = new List<char> { 'H', 'T' };
        List<string> commandTrail = new List<string>();
        Stack<List<char>> commandsStack = new Stack<List<char>>();
        public List<int> boosterPositions = new List<int>();
        public List<int> obstaclePositions = new List<int>();
        public List<int> spicePositions = new List<int>();
        protected ILogger _logger;

        public Caterpillar()
        {
            Length = 1;
            _logger = Log.ForContext(GetType());
        }

        // Capterpillar command functions

        private void MoveUp()
        {
            if (headIndex > 0)
            {
                if (!IsObstacle(headIndex - 1))
                {
                    headIndex--;

                    // Shift other segments up
                    for (int i = 1; i < segments.Count; i++)
                    {
                        segments[i] = segments[i - 1];
                    }
                }
                else
                {
                    _logger.Error("Obstacle detected! Caterpillar disintegrated.");
                    Environment.Exit(0);
                }
            }

            logCommands($"Move Up");
        }

        private void MoveDown()
        {
            if (headIndex < segments.Count - 1)
            {
                if (!IsObstacle(headIndex + 1))
                {
                    headIndex++;
                }
                else
                {
                    _logger.Error("Obstacle detected! Caterpillar disintegrated.");
                    Environment.Exit(0);
                }
            }

            logCommands($"Move Down");
        }

        private void MoveLeft()
        {
            if (!IsBooster(headIndex - 1))
            {
                // Move the head left
                headIndex--;

                // Shift other segments left
                for (int i = 0; i < segments.Count - 1; i++)
                {
                    segments[i] = segments[i + 1];
                }
            }
            else
            {
                _logger.Information("Booster consumed! Caterpillar grows.");
                if (segments.Count < 5)
                {
                    segments.Add('B');
                }
            }

            logCommands($"Move Left");
        }

        private void MoveRight()
        {
            if (!IsSpice(headIndex + 1))
            {
                // Move the head right
                headIndex++;

                // Shift other segments right
                for (int i = segments.Count - 1; i > 0; i--)
                {
                    segments[i] = segments[i - 1];
                }
            }
            else
            {
                _logger.Information("Spice ingested!");
            }
            logCommands($"Move Right");
        }

        public void Grow()
        {
            if (Length < 5)
            {
                segments.Add('B');
                commandsStack.Push(new List<char>(segments));
                Length++;
                logCommands("Grow");
            }
            else
            {
                _logger.Error("Cannot grow beyond 5 segments.");
            }
        }
        public void Shrink()
        {
            if (Length > 2)
            {
                segments.RemoveAt(segments.Count - 1);
                commandsStack.Push(new List<char>(segments));
                Length--;
                logCommands("Shrink");
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

        // Execute rider commands and logs it in a command trail

        public void ExecuteRiderCommand(char riderCommand, int steps = 0)
        {
            switch (riderCommand)
            {
                case 'U':
                    for (int i = 0; i < steps; i++)
                        MoveUp();
                    break;
                case 'D':
                    for (int i = 0; i < steps; i++)
                        MoveDown();
                    break;
                case 'L':
                    for (int i = 0; i < steps; i++)
                        MoveLeft();
                    break;
                case 'R':
                    for (int i = 0; i < steps; i++)
                        MoveRight();
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

        public bool IsObstacle(int obstaclePosition)
        {
            return obstaclePositions.Contains(obstaclePosition);

        }
        public bool IsBooster(int position)
        {
            return boosterPositions.Contains(position);
        }

        public bool IsSpice(int position)
        {
            return spicePositions.Contains(position);
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

            // Mark caterpillar segments on the grid
            foreach (var segment in segments)
            {
                int x = headIndex + segments.IndexOf(segment);
                int y = GRID_SIZE / 2; // Center vertically
                if (x >= 0 && x < GRID_SIZE)
                {
                    landGrid[y, x] = segment;
                }
            }

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
    }
}
