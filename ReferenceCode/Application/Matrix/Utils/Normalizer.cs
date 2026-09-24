namespace Application.Matrix.Utils;

public static class Normalizer
{
    public static string? NormalizeInput(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        return value;
    }
}