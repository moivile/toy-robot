using Domain.Enums;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ToyRobot.Tests.Domain.ValueObjects
{
    public class RobotStateTests
    {
        [Theory]
        [InlineData(-1, 1, DirectionEnum.North)]
        public void X_Should_Be_Positive(int x, int y, DirectionEnum direction)
        {
            // Arrange

            // Act
            var state = RobotState.Create(x, y, direction);

            // Assert
            state.IsFailure.Should().Be(true);
        }

        [Theory]
        [InlineData(5, 1, DirectionEnum.North)]
        public void X_Should_Be_Less_Than_AxisWidth(int x, int y, DirectionEnum direction)
        {
            // Arrange

            // Act
            var state = RobotState.Create(x, y, direction);

            // Assert
            state.IsFailure.Should().Be(true);
        }

        [Fact]
        public void Robot_States_Are_Equal_If_They_Values_Are_Equal()
        {
            // Arrange
            const int x = 1;
            const int y = 2;
            const DirectionEnum direction = DirectionEnum.East;

            // Act
            var state1 = RobotState.Create(x, y, direction);
            var state2 = RobotState.Create(x, y, direction);

            // Assert
            state1.Value.Equals(state2.Value).Should().Be(true);

        }

        [Theory]
        [InlineData(2, 2, DirectionEnum.East)]
        [InlineData(1, 3, DirectionEnum.East)]
        [InlineData(1, 2, DirectionEnum.North)]

        public void Robot_States_Are_Not_Equal_If_They_Values_Are_Not_Equal(int x1, int y1, DirectionEnum direction1)
        {
            // Arrange
            const int x2 = 1;
            const int y2 = 2;
            const DirectionEnum direction2 = DirectionEnum.East;

            // Act
            var state1 = RobotState.Create(x1, y1, direction1);
            var state2 = RobotState.Create(x2, y2, direction2);

            // Assert
            state1.Value.Equals(state2.Value).Should().Be(false);
        }

    }
}
