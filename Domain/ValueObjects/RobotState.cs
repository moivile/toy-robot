using System.Collections.Generic;
using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects
{
    public class RobotState : ValueObject
    {
        public const int AXIS_WIDTH = 5;
        public int X { get; }
        public int Y { get; }
        public DirectionEnum Direction { get; }

        private RobotState(int x, int y, DirectionEnum direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public static Result<RobotState> Create(int x, int y, DirectionEnum direction)
        {
            if (x < 0 || 4 < x)
            {
                return Result.Failure<RobotState>(new Error(
                    "RobotState.X.OutOfRange",
                    $"RobotState.X is less than 0 or more than {AXIS_WIDTH - 1}."));
            }

            if (y < 0 || 4 < y)
            {
                return Result.Failure<RobotState>(new Error(
                    "RobotState.Y.OutOfRange",
                    $"RobotState.Y is less than 0 or more than {AXIS_WIDTH - 1}."));
            }

            return new RobotState(x, y, direction);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return X;
            yield return Y;
            yield return Direction;
        }
    }
}
