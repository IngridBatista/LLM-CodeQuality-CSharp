namespace Application.Sequence.StopConditions;

public interface IStopCondition<in T>
{
    bool IsStop(T value);
}