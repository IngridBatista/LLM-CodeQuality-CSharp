using Application.Sequence.Types;

namespace Application.Sequence.StopConditions;

public sealed class StopOnNegativeFraction : IStopCondition<Fraction>
{
    public bool IsStop(Fraction value) => value.GetValue() < 0;
}