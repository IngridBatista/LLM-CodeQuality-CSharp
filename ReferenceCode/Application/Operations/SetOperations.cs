using Domain.Interfaces;

namespace Application.Operations;

public class SetOperations : ISetOperations
{
    public int[] Difference(int[] a, int[] b)
    {
        HashSet<int> setB = new HashSet<int>(b);
        List<int> result = new List<int>();

        foreach (int item in a)
            if (!setB.Contains(item))
                result.Add(item);

        return result.ToArray();
    }
}