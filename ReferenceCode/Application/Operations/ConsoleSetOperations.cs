using Application.Operations.Utils;
using Application.Operations.Resources;
using Domain.Interfaces;

namespace Application.Operations;

public class ConsoleSetOperations : IConsole
{
    private readonly ISetOperations _setOperations;

    public ConsoleSetOperations(ISetOperations setOperations)
    {
        _setOperations = setOperations;
    }

    public void Execute()
    {
        Console.Write(SetOperationsMessages.EnterFirstArray);
        string? input = Console.ReadLine().NormalizeInput();
        int[] a = Parser.ParseArray(input);

        Console.Write(SetOperationsMessages.EnterSecondArray);
        input = Console.ReadLine().NormalizeInput();
        int[] b = Parser.ParseArray(input);

        int[] result = _setOperations.Difference(a, b);

        Console.WriteLine(string.Format(SetOperationsMessages.ResultArrayA, string.Join(", ", a)));
        Console.WriteLine(string.Format(SetOperationsMessages.ResultArrayB, string.Join(", ", b)));
        Console.WriteLine(string.Format(SetOperationsMessages.ResultDifference, string.Join(", ", result)));
    }
}
