using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Robot: IRobot
    {
        public RobotState? State { get; private set; }

        public void Place(RobotState newState)
        {
            State = newState;
        }

        public Result Move()
        {
            if (State is null)
            {
                return _stateIsNullFailureResult;
            }

            var newX = State.X;
            var newY = State.Y;
            if (State.Direction == DirectionEnum.North)
            {
                ++newY;
            }
            else if (State.Direction == DirectionEnum.East)
            {
                ++newX;
            }
            else if (State.Direction == DirectionEnum.South)
            {
                --newY;
            }
            else if (State.Direction == DirectionEnum.West)
            {
                --newX;
            }

            var newState = RobotState.Create(newX, newY, State.Direction);
            if (newState.IsSuccess)
            {
                State = newState.Value;
                return Result.Success();
            }
            return Result.Failure(newState.Error);
        }

        public Result Left()
        {
            if (State is null)
            {
                return _stateIsNullFailureResult;
            }

            var newDirection = (DirectionEnum)(State.Direction == 0 ? 3 : (int)State.Direction - 1);
            var newState = RobotState.Create(State.X, State.Y, newDirection);
            State = newState.Value;

            return Result.Success();
        }

        public Result Right()
        {
            if (State is null)
            {
                return _stateIsNullFailureResult;
            }

            var newDirection = (DirectionEnum)((int)State.Direction == 3 ? 0 : (int)State.Direction + 1);
            var newState = RobotState.Create(State.X, State.Y, newDirection);
            State = newState.Value;

            return Result.Success();
        }

        private readonly Result _stateIsNullFailureResult = Result.Failure(new Error(
                "Robot.RobotState.NullReference",
                $"State has not been initialized."));
    }
}
