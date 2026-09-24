namespace Application.Sequence.Factories;

using Application.Sequence.StopConditions;

public interface IStopConditionFactory
{
    IStopCondition<T> Create<T>();
}
