namespace Application.CodeMetrics;

using System.Globalization;
using Application.Sequence.Factories;
using Application.Sequence.Resources;
using Application.Sequence.Types;
using Application.Sequence.Utils;
using Domain.Interfaces;

public class CompareSequence : IConsole
{
    private readonly IStopConditionFactory _stopConditionFactory;

    public CompareSequence(IStopConditionFactory stopConditionFactory)
    {
        _stopConditionFactory = stopConditionFactory;
    }

    public void Execute()
    {
        List<double> sequenceDouble = ReadSequence<double>();
        List<Fraction> sequenceFraction = ReadSequence<Fraction>();

        if (sequenceDouble.Count == 0 || sequenceFraction.Count == 0)
        {
            Console.WriteLine(SequenceMessages.EmptySequencesError);
            return;
        }

        List<Fraction> result = GetFractionsGreaterThanHalfOfA(sequenceDouble, sequenceFraction);

        Console.WriteLine($"\n");
        Console.WriteLine(SequenceMessages.Separator);
        Console.WriteLine(SequenceMessages.ResultTitle);
        Console.WriteLine(SequenceMessages.Separator);

        if (result.Count == 0)
        {
            Console.WriteLine(SequenceMessages.NoMatchingFractions);
        }
        else
        {
            foreach (var fraction in result)

                Console.WriteLine($"{fraction} = {fraction.GetValue():F2}");
        }
    }

    private List<Fraction> GetFractionsGreaterThanHalfOfA(List<double> sequenceA, List<Fraction> sequenceB)
    {
        int halfOfA = (sequenceA.Count + 1) / 2;

        return sequenceB
            .Where(fraction => sequenceA.Count(value => fraction.IsGreater(value)) > halfOfA)
            .ToList();
    }

    private List<T> ReadSequence<T>() where T : IParsable<T>
    {
        List<T> sequence = new List<T>();
        var typeName = typeof(T).Name;

        Console.WriteLine(SequenceMessages.GetSequenceTitle(typeName));
        Console.WriteLine(SequenceMessages.GetInputHint(typeName));

        while (true)
        {
            Console.Write(SequenceMessages.InputPrompt);
            if (T.TryParse(Console.ReadLine()?.NormalizeInput(), CultureInfo.InvariantCulture, out T? value))
            {
                if (_stopConditionFactory.Create<T>().IsStop(value))
                {
                    Console.WriteLine(SequenceMessages.ReadingFinished);
                    break;
                }

                sequence.Add(value);
            }
            else
            {
                Console.WriteLine(SequenceMessages.GetInvalidInputMessage(typeName));
            }
        }

        return sequence;
    }
}