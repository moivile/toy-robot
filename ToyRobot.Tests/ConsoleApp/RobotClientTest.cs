using ConsoleApp;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ToyRobot.Tests.ConsoleApp
{
    public class RobotClientTest
    {
        private readonly RobotClientFixture _fixture;

        public RobotClientTest()
        {
            _fixture = new RobotClientFixture();
        }

        [Theory]
        [InlineData("PLACE 2,2,NORTH", 2, 2, DirectionEnum.North)]
        [InlineData("Place 1,0,East", 1, 0, DirectionEnum.East)]
        [InlineData("place 3,2,south", 3, 2, DirectionEnum.South)]
        [InlineData("place 2,0,west", 2, 0, DirectionEnum.West)]
        public void RobotClient_Should_Perform_Place_Command(string input, int expX, int expY, DirectionEnum expDirection)
        {
            // Arrange
            var sut = _fixture.CreateSut();
            var expectedState = RobotState.Create(expX, expY, expDirection).Value;

            // Act
            var result = sut.RunCommand(input);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _fixture.Robot.Verify(x => x.Place(expectedState));
        }

        [Theory]
        [InlineData("MOVE")]
        [InlineData("LEFT")]
        [InlineData("RIGHT")]
        [InlineData("REPORT")]
        public void The_First_Command_Should_Be_A_Place_Command(string input)
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.RunCommand(input);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData("MOVE")]
        [InlineData("move")]
        [InlineData("Move")]
        public void RobotClient_Should_Perform_Move_Command(string input)
        {
            // Arrange
            var sut = _fixture.WithInitializedRobotState().CreateSut();

            // Act
            var result = sut.RunCommand(input);

            // Assert
            _fixture.Robot.Verify(x => x.Move());
        }

        [Theory]
        [InlineData("LEFT")]
        [InlineData("Left")]
        [InlineData("left")]
        public void RobotClient_Should_Perform_Left_Command(string input)
        {
            // Arrange
            var sut = _fixture.WithInitializedRobotState().CreateSut();

            // Act
            var result = sut.RunCommand(input);

            // Assert
            _fixture.Robot.Verify(x => x.Left());
        }

        [Theory]
        [InlineData("RIGHT")]
        [InlineData("Right")]
        [InlineData("right")]
        public void RobotClient_Should_Perform_Right_Command(string input)
        {
            // Arrange
            var sut = _fixture.WithInitializedRobotState().CreateSut();

            // Act
            var result = sut.RunCommand(input);

            // Assert
            _fixture.Robot.Verify(x => x.Right());
        }

        [Theory]
        [InlineData("REPORT")]
        [InlineData("Report")]
        [InlineData("report")]
        public void RobotClient_Should_Perform_Report_Command(string input)
        {
            // Arrange
            var sut = _fixture.WithInitializedRobotState().CreateSut();

            // Act
            var result = sut.RunCommand(input);

            // Assert
            var expectedMessage = $"Output: {sut.State.X},{sut.State.Y},{sut.State.Direction}";
            _fixture.ReportService.Verify(x => x.Report(expectedMessage));
        }

    }

    internal class RobotClientFixture
    {
        public Mock<IReportService> ReportService { get; set; }
        public Mock<IRobot> Robot { get; set; }

        public RobotClientFixture()
        {
            ReportService = new Mock<IReportService>();
            Robot = new Mock<IRobot>();
        }

        public RobotClient CreateSut()
        {
            return new RobotClient(ReportService.Object, Robot.Object);
        }

        public RobotClientFixture WithInitializedRobotState()
        {
            Robot.Setup(x => x.State).Returns(RobotState.Create(0, 0, DirectionEnum.North).Value);
            return this;
        }
    }
}
