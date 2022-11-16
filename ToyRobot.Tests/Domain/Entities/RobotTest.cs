using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using FluentAssertions;

namespace ToyRobot.Tests.Domain.Entities
{
    public class RobotTest
    {
        [Theory]
        [InlineData(2, 2, DirectionEnum.East)]
        [InlineData(1, 3, DirectionEnum.East)]
        [InlineData(1, 2, DirectionEnum.North)]
        public void Robot_Should_Perform_Place_Command(int x, int y, DirectionEnum direction)
        {
            // Arrange
            var robot = new Robot();
            var newState = RobotState.Create(x, y, direction).Value;

            // Act
            robot.Place(newState);

            // Assert
            robot.State.Equals(newState).Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 3, DirectionEnum.North, 1, 4)]
        [InlineData(1, 4, DirectionEnum.North, 1, 4)]
        [InlineData(2, 2, DirectionEnum.East, 3, 2)]
        [InlineData(4, 2, DirectionEnum.East, 4, 2)]
        [InlineData(1, 2, DirectionEnum.South, 1, 1)]
        [InlineData(1, 0, DirectionEnum.South, 1, 0)]
        [InlineData(1, 4, DirectionEnum.West, 0, 4)]
        [InlineData(0, 4, DirectionEnum.West, 0, 4)]
        public void Robot_Should_Perform_Move_Command_Only_If_Result_Coordinates_Are_Valid(int initX, int initY, DirectionEnum initDirection, int resX, int resY)
        {
            // Arrange
            var initialState = RobotState.Create(initX, initY, initDirection).Value;
            var robot = new Robot();
            robot.Place(initialState);


            // Act
            robot.Move();

            // Assert
            var expectedState = RobotState.Create(resX, resY, initDirection);
            if (expectedState.IsSuccess)
            {
                robot.State.Equals(expectedState.Value).Should().BeTrue();
            }
            else
            {
                robot.State.Equals(initialState).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(1, 3, DirectionEnum.North)]
        [InlineData(2, 2, DirectionEnum.East)]
        [InlineData(1, 2, DirectionEnum.South)]
        [InlineData(1, 0, DirectionEnum.West)]
        public void Move_Command__Should_Return_Success_If_Result_Coordinates_Are_Valid(int initX, int initY, DirectionEnum initDirection)
        {
            // Arrange
            var initialState = RobotState.Create(initX, initY, initDirection);
            var robot = new Robot();
            robot.Place(initialState.Value);

            // Act
            var result = robot.Move();

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Move_Command__Should_Validate_State()
        {
            // Arrange
            var robot = new Robot();

            // Act
            var result = robot.Move();

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 4, DirectionEnum.North)]
        [InlineData(4, 1, DirectionEnum.East)]
        [InlineData(1, 0, DirectionEnum.South)]
        [InlineData(0, 2, DirectionEnum.West)]
        public void Move_Command__Should_Return_Error_If_Result_Coordinates_Are_Invalid(int initX, int initY, DirectionEnum initDirection)
        {
            // Arrange
            var initialState = RobotState.Create(initX, initY, initDirection);
            var robot = new Robot();
            robot.Place(initialState.Value);

            // Act
            var result = robot.Move();

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(DirectionEnum.North, DirectionEnum.East)]
        [InlineData(DirectionEnum.East, DirectionEnum.South)]
        [InlineData(DirectionEnum.South, DirectionEnum.West)]
        [InlineData(DirectionEnum.West, DirectionEnum.North)]
        public void Robot_Should_Perform_Right_Command(DirectionEnum initDirection, DirectionEnum resDirection)
        {
            // Arrange
            var initialState = RobotState.Create(1, 1, initDirection);
            var robot = new Robot();
            robot.Place(initialState.Value);

            // Act
            robot.Right();

            // Assert
            var expectedState = RobotState.Create(1, 1, resDirection);
            robot.State.Equals(expectedState.Value).Should().BeTrue();
        }

        [Fact]
        public void Right_Command_Should_Validate_State()
        {
            // Arrange
            var robot = new Robot();

            // Act
            var result = robot.Right();

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(DirectionEnum.East, DirectionEnum.North)]
        [InlineData(DirectionEnum.North, DirectionEnum.West)]
        [InlineData(DirectionEnum.West, DirectionEnum.South)]
        [InlineData(DirectionEnum.South, DirectionEnum.East)]
        public void Robot_Should_Perform_Left_Command(DirectionEnum initDirection, DirectionEnum resDirection)
        {
            // Arrange
            var initialState = RobotState.Create(1, 1, initDirection);
            var robot = new Robot();
            robot.Place(initialState.Value);

            // Act
            robot.Left();

            // Assert
            var expectedState = RobotState.Create(1, 1, resDirection);
            robot.State.Equals(expectedState.Value).Should().BeTrue();
        }

        [Fact]
        public void Left_Command_Should_Validate_State()
        {
            // Arrange
            var robot = new Robot();

            // Act
            var result = robot.Left();

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
