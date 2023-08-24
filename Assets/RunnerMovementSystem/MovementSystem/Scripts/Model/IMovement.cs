
namespace RunnerMovementSystem.Model
{
    public interface IMovement
    {
        float Offset { get; }
        PathSegment PathSegment { get; }

        void Update();
        void MoveForward();
        void SetOffset(float offset);
    }
}