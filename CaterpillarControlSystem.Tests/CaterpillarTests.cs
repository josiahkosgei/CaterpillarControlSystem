using CaterpillarControlSystem.App;
using NUnit.Framework.Internal;

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
            _caterpillar = new Caterpillar
            {
                obstacleLocations =
            [
                new (1, 18),
                new (2, 25),
                new (3, 24),
            ],
                spiceLocations =
            [
                new (1, 8),
                new (2, 15),
                new (3, 4),
            ],
                boosterLocations =
            [
                new (4, 18),
                new (15, 25),
                new (20, 24),
            ]
            };
        }
        [Test]
        public void InitialCaterpillarLength_Should_ReturnEqual_When_Initiated()
        {
            // Arrange
            _caterpillar = new Caterpillar();
            // Act

            // Assert
            Assert.That(_caterpillar.segmentPosition, Has.Count.EqualTo(2));
        }
        [Test]
        public void InitialCaterpillarLength_Should_ReturnNotEqual_When_Initiated()
        {
            //Arrange
            _caterpillar = new Caterpillar();
            // Assert
            Assert.That(_caterpillar.segmentPosition, Has.Count.Not.EqualTo(1));
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
            Assert.That(_caterpillar.headX, Is.EqualTo(-4));
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
            Assert.That(_caterpillar.headX, Is.EqualTo(1));
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
            Assert.That(_caterpillar.headY, Is.EqualTo(-3));
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
            Assert.That(_caterpillar.headY, Is.EqualTo(3));
        }

        [Test]
        public void GrowCaterpillar_Should_ReturnEqual()
        {

            // Arrange
            _caterpillar = new Caterpillar();

            // Act
            _caterpillar.Grow();

            int distanceY = Math.Abs(_caterpillar.headY - _caterpillar.tailY);
            // Assert
            Assert.That(distanceY, Is.EqualTo(1));
        }

        [Test]
        public void TestHead_Tail_X_Distance()
        {

            // Arrange

            _caterpillar = new Caterpillar();
            // Act

            _caterpillar.Move("R", steps: 3);
            _caterpillar.Move("D", steps: 2);
            int distanceX = Math.Abs(_caterpillar.headX - _caterpillar.tailX);

            // Assert
            Assert.That(distanceX, Is.EqualTo(1));

        }
        [Test]
        public void TestHead_Tail_Y_Distance()
        {

            // Arrange

            _caterpillar = new Caterpillar();
            // Act

            _caterpillar.Move("R", steps: 3);
            _caterpillar.Move("D", steps: 2);

            int distanceY = Math.Abs(_caterpillar.headY - _caterpillar.tailY);

            // Assert
            Assert.That(distanceY, Is.EqualTo(1));
        }

        [Test]
        public void ValidateDiagonalTailMovement()
        {

            // Arrange
            _caterpillar = new Caterpillar();

            // Act
            _caterpillar.Move("R", steps: 3);
            _caterpillar.Move("D", steps: 2);

            // Assert
            Assert.That(_caterpillar.segmentPosition[0], Is.EqualTo((2, 3)));
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

            int distanceY = Math.Abs(_caterpillar.headY - _caterpillar.tailY);
            // Assert
            Assert.That(distanceY, Is.EqualTo(2));
        }

        [Test]
        public void TestInvalidCommand()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _caterpillar.ExecuteRiderCommand('X', 0));
        }

        // Collision Detection Tests
        [Test]
        public void ObstacleCollision_Should_ReturnTrue_When_ObstacleDetected()
        {
            // Arrange
            (int x, int y) obstaclePosition = new(2, 25); // Assuming obstacle position
            // Act
            bool result = _caterpillar.ObstacleCollision(obstaclePosition.x, obstaclePosition.y);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ObstacleCollision_Should_ReturnFalse_When_NoObstacleDetected()
        {
            // Arrange
            (int x, int y) nonObstaclePosition = new(2, 3);

            // Act
            bool result = _caterpillar.ObstacleCollision(nonObstaclePosition.x, nonObstaclePosition.y);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void BoosterCollision_Should_ReturnTrue_When_BoosterDetected()
        {
            // Arrange
            (int x, int y) boosterPosition = new(4, 18); // Assuming booster position

            // Act
            bool result = _caterpillar.BoosterCollision(boosterPosition.x, boosterPosition.y);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void BoosterCollision_Should_ReturnFalse_When_NoBoosterDetected()
        {
            // Arrange
            (int x, int y) nonBoosterPosition = new(18, 18); // Assuming non-booster position

            // Act
            bool result = _caterpillar.BoosterCollision(nonBoosterPosition.x, nonBoosterPosition.y);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void SpiceCollision_Should_ReturnTrue_When_SpiceDetected()
        {
            // Arrange
            (int x, int y) spicePosition = new(2, 15); // Assuming spice position

            // Act
            bool result = _caterpillar.SpiceCollision(spicePosition.x, spicePosition.y);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void SpiceCollision_Should_ReturnFalse_When_NoSpiceDetected()
        {
            // Arrange
            (int x, int y) nonSpicePosition = new(18, 18); // Assuming non-spice position

            // Act
            bool result = _caterpillar.SpiceCollision(nonSpicePosition.x, nonSpicePosition.y);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
