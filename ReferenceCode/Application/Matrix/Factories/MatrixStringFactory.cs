using Domain.Interfaces;

namespace Application.Matrix.Factories;

public class MatrixStringFactory : IMatrixStringFactory
{
    private readonly Func<int, int, string?, IMatrixString> _createFunc;

    public MatrixStringFactory(Func<int, int, string?, IMatrixString> createFunc)
    {
        _createFunc = createFunc;
    }

    public IMatrixString Create(int rows, int columns, string? value)
    {
        return _createFunc(rows, columns, value);
    }
}
