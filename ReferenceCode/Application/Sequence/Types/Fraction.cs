using System.Diagnostics.CodeAnalysis;

namespace Application.Sequence.Types;

public class Fraction : IParsable<Fraction>
{
    public int Numerator { get; private set; }
    public int Denominator { get; private set; }

    public Fraction Of(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Denominador não pode ser zero.");

        Numerator = numerator;
        Denominator = denominator;

        return this;
    }

    public double GetValue()
    {
        return (double)Numerator / Denominator;
    }

    public bool IsLesser(double value)
    {
        return GetValue() < value;
    }

    public bool IsGreater(double value)
    {
        var getValue = GetValue();
        bool isGreater = getValue > value;
        return isGreater;
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public static Fraction Parse(string s, IFormatProvider? provider)
    {
        string[] parts = s.Split('/');
        if (parts.Length != 2)
            throw new FormatException("Formato inválido! Use: numerador/denominador. Exemplo 1/2");

        if (!int.TryParse(parts[0], out int numerator) || !int.TryParse(parts[1], out int denominator))
            throw new FormatException("Valores inválidos! Digite números inteiros");

        if (denominator == 0)
            throw new ArgumentException("Denominador não pode ser zero");

        return new Fraction().Of(numerator, denominator);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
    {
        result = null;
        if (s == null)
            return false;

        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
