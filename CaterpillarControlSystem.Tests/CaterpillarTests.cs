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
            _caterpillar.obstaclePositions = [5, 12, 18];
            _caterpillar.spicePositions = [10, 15, 20];
            _caterpillar.boosterPositions = [5, 12, 18];
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
            _caterpillar= new Caterpillar();

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
        public void IsObstacle_Should_ReturnTrue_When_ObstacleDetected()
        {
            // Arrange
            int obstaclePosition = 5; // Assuming obstacle position
            // Act
            bool result = _caterpillar.IsObstacle(obstaclePosition);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsObstacle_Should_ReturnFalse_When_NoObstacleDetected()
        {
            // Arrange
            int nonObstaclePosition = 3; // Assuming non-obstacle position
            // Act
            bool result = _caterpillar.IsObstacle(nonObstaclePosition);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
