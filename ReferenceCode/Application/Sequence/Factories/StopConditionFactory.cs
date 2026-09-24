using Application.Sequence.StopConditions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Sequence.Factories;

public class StopConditionFactory : IStopConditionFactory
{
    private readonly IServiceProvider _serviceProvider;

    public StopConditionFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IStopCondition<T> Create<T>()
    {
        var typeName = typeof(T).Name.ToLower();
        return _serviceProvider.GetRequiredKeyedService<IStopCondition<T>>(typeName);
    }
}
