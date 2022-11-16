using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Robot: IRobot
    {
        private RobotState? _state { get; set; }

        public RobotState? State => _state;

        public void Place(RobotState newState)
        {
            _state = newState;
        }

        public Result Move()
        {
            if (_state is null)
            {
                return StateIsNullFailureResult;
            }

            var newX = _state.X;
            var newY = _state.Y;
            if (_state.Direction == DirectionEnum.North)
            {
                ++newY;
            }
            else if (_state.Direction == DirectionEnum.East)
            {
                ++newX;
            }
            else if (_state.Direction == DirectionEnum.South)
            {
                --newY;
            }
            else if (_state.Direction == DirectionEnum.West)
            {
                --newX;
            }

            var newState = RobotState.Create(newX, newY, _state.Direction);
            if (newState.IsSuccess)
            {
                _state = newState.Value;
                return Result.Success();
            }
            return Result.Failure(newState.Error);
        }

        public Result Left()
        {
            if (_state is null)
            {
                return StateIsNullFailureResult;
            }

            var newDirection = (DirectionEnum)(_state.Direction == 0 ? 3 : (int)_state.Direction - 1);
            var newState = RobotState.Create(_state.X, _state.Y, newDirection);
            _state = newState.Value;

            return Result.Success();
        }

        public Result Right()
        {
            if (_state is null)
            {
                return StateIsNullFailureResult;
            }

            var newDirection = (DirectionEnum)((int)_state.Direction == 3 ? 0 : (int)_state.Direction + 1);
            var newState = RobotState.Create(_state.X, _state.Y, newDirection);
            _state = newState.Value;

            return Result.Success();
        }

        private readonly Result StateIsNullFailureResult = Result.Failure(new Error(
                "Robot.RobotState.NullReference",
                $"State has not been initialized."));
    }
}
