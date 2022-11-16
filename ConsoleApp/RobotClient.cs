using System;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Domain.Shared;
using Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    public class RobotClient
    {
        private readonly IReportService _reportService;
        private readonly IRobot _robot;

        public RobotState? State => _robot.State;

        public RobotClient(IReportService reportService, IRobot robot)
        {
            _reportService = reportService;
            _robot = robot;
        }

        public Result RunCommand(string input)
        {
            var match = Regex.Match(input, @"PLACE (\d),(\d),(NORTH|EAST|SOUTH|WEST)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var direction = Enum.Parse<Direction>(match.Groups[3].Value, true);

                var newState = RobotState.Create(x, y, direction);

                if (newState.IsFailure)
                {
                    return Result.Failure(newState.Error);
                }

                _robot.Place(newState.Value);


                return Result.Success();
            }

            if (_robot.State is null)
            {
                return Result.Failure(new Error(
                    "RobotClient.RunCommand.InvalidOperation",
                    $"The first command must be a correct PLACE command."));
            }

            match = Regex.Match(input, "MOVE", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var result = _robot.Move();
                return result;
            }

            match = Regex.Match(input, "LEFT", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var result = _robot.Left();
                return result;
            }

            match = Regex.Match(input, "RIGHT", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var result = _robot.Right();
                return result;
            }

            match = Regex.Match(input, "REPORT", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                _reportService.Report($"Output: {_robot.State.X},{_robot.State.Y},{_robot.State.Direction}");
                return Result.Success();
            }


            return Result.Failure(new Error(
                "RobotClient.RunCommand.InvalidOperation",
                $"The command has not been recognized."));
        }
    }
}
