using Application.CodeMetrics;
using Application.Operations;
using Application.Sequence.StopConditions;
using Application.Sequence.Factories;
using Microsoft.Extensions.DependencyInjection;
using Application.Sequence.Types;
using Application.Matrix;
using Domain.Interfaces;
using Domain.Constants;
using Presentation;
using Application.Matrix.Factories;

namespace CodeMetrics;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();

        var app = new ConsoleApplication(serviceProvider);
        app.Run();
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddKeyedTransient<IStopCondition<double>, DoubleStop>(ServiceKeys.DoubleStopCondition);
        services.AddKeyedTransient<IStopCondition<Fraction>, StopOnNegativeFraction>(ServiceKeys.FractionStopCondition);
        services.AddSingleton<IStopConditionFactory, StopConditionFactory>();

        services.AddTransient<IMatrixString, MatrixString>();
        services.AddTransient<IMatrixStringFactory>(sp =>
            new MatrixStringFactory((rows, columns, value) => new MatrixString(rows, columns, value)));

        services.AddTransient<ISetOperations, SetOperations>();

        services.AddKeyedTransient<IConsole, ConsoleSetOperations>(ServiceKeys.SetOperations);
        services.AddKeyedTransient<IConsole, CompareSequence>(ServiceKeys.CompareSequence);
        services.AddKeyedTransient<IConsole, ConsoleMatrix>(ServiceKeys.ConsoleMatrix);

        return services.BuildServiceProvider();
    }
}
