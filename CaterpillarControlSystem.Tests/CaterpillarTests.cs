using CaterpillarControlSystem.App;

namespace CaterpillarControlSystem.Tests
{
    [TestFixture]
    public class CaterpillarTests
    {
        private Caterpillar _caterpillar;

        [SetUp]
        public void Setup()
        {
            // Initialize a caterpillar for testing
            _caterpillar = new Caterpillar();

            _caterpillar.obstacleLocations = new List<(int, int)>()
            {
                new (1, 18),
                new (2, 25),
                new (3, 24),
            };
            _caterpillar.spiceLocations = new List<(int, int)>()
            {
                new (1, 8),
                new (2, 15),
                new (3, 4),
            };
            _caterpillar.boosterLocations = new List<(int, int)>()
            {
                new (4, 18),
                new (15, 25),
                new (20, 24),
            };
        }
        [Test]
        public void InitialCaterpillarLength_Should_ReturnEqual_When_Initiated()
        {
            // Arrange
            // Act
            // Assert
            Assert.That(_caterpillar.GetLength(), Is.EqualTo(1));
        }
        [Test]
        public void InitialCaterpillarLength_Should_ReturnNotEqual_When_Initiated()
        {
            // Assert
            Assert.That(2, Is.Not.EqualTo(_caterpillar.GetLength()));
        }

        [Test]
        public void TestExecuteRiderCommandUp()
        {
            // Arrange
            char riderCommand = 'U';
            int steps = 4;

            // Act
            _caterpillar.ExecuteRiderCommand(riderCommand, steps);

            // Assert
            //  Assert.AreEqual((0, 1), _caterpillar.HeaderPosition);
        }

        [Test]
        public void TestExecuteRiderCommandDown()
        {
            // Arrange
            char riderCommand = 'D';
            int steps = 1;

            // Act
            _caterpillar.ExecuteRiderCommand(riderCommand, steps);

            // Assert
            //  Assert.AreEqual((0, -1), _caterpillar.HeaderPosition);
        }

        [Test]
        public void TestExecuteRiderCommandLeft()
        {

            // Arrange
            char riderCommand = 'L';
            int steps = 3;

            // Act
            _caterpillar.ExecuteRiderCommand(riderCommand, steps);

            // Assert
            //  Assert.AreEqual((-1, 0), _caterpillar.HeaderPosition);
        }

        [Test]
        public void TestExecuteRiderCommandRight()
        {

            // Arrange
            char riderCommand = 'R';
            int steps = 3;

            // Act
            _caterpillar.ExecuteRiderCommand(riderCommand, steps);

            // Assert
            //  Assert.AreEqual((1, 0), _caterpillar.HeaderPosition);
        }

        [Test]
        public void GrowCaterpillar_Should_ReturnEqual()
        {

            // Arrange
            _caterpillar = new Caterpillar();

            // Act
            _caterpillar.Grow();

            // Assert
            Assert.That(_caterpillar.Length, Is.EqualTo(2));
        }

        [Test]
        public void ShrinkCaterpillar_Should_ReturnEqual()
        {

            // Arrange
            _caterpillar = new Caterpillar();

            // Act
            _caterpillar.Grow();
            _caterpillar.Grow();
            _caterpillar.Grow();
            _caterpillar.Shrink();

            // Assert
            Assert.That(_caterpillar.Length, Is.EqualTo(3));
        }

        [Test]
        public void TestInvalidCommand()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _caterpillar.ExecuteRiderCommand('X', 0));
        }

        [Test]
        public void ObstacleCollision_Should_ReturnTrue_When_ObstacleDetected()
        {
            // Arrange
            (int x, int y) obstaclePosition = new(2, 25); // Assuming obstacle position
            // Act
            bool result = _caterpillar.ObstacleCollision(obstaclePosition.x, obstaclePosition.y);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ObstacleCollision_Should_ReturnFalse_When_NoObstacleDetected()
        {
            // Arrange
            (int x, int y) nonObstaclePosition = new(2, 3);

            // Act
            bool result = _caterpillar.ObstacleCollision(nonObstaclePosition.x, nonObstaclePosition.y);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void BoosterCollision_Should_ReturnTrue_When_BoosterDetected()
        {
            // Arrange
            (int x, int y) boosterPosition = new(4, 18); // Assuming booster position

            // Act
            bool result = _caterpillar.BoosterCollision(boosterPosition.x, boosterPosition.y);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BoosterCollision_Should_ReturnFalse_When_NoBoosterDetected()
        {
            // Arrange
            (int x, int y) nonBoosterPosition = new(18, 18); // Assuming non-booster position

            // Act
            bool result = _caterpillar.BoosterCollision(nonBoosterPosition.x, nonBoosterPosition.y);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
