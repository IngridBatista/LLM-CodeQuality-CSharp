namespace Application.Operations.Utils;

public static class Parser
{
    public static int[] ParseArray(string? input)
    {
        return input?.Split(',')
            .Select(s => s.Trim())
            .Where(s => int.TryParse(s, out _))
            .Select(int.Parse)
            .ToArray() ?? Array.Empty<int>();
    }
}