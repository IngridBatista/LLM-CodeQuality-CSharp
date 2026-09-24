namespace Application.Sequence.Utils;

public static class Normalizer
{
    public static string NormalizeInput(this string value)
    {
        string line = value?.Replace(",", ".").Trim() ?? string.Empty;

        return line;
    }
}