using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public interface IRobot
    {
        RobotState? State { get; }
        void Place(RobotState newState);
        Result Move();
        Result Left();
        Result Right();
    }
}
