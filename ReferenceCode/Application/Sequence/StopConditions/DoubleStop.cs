namespace Application.Sequence.StopConditions;

public sealed class DoubleStop : IStopCondition<double>
{
    public bool IsStop(double value) => value == 0;
}