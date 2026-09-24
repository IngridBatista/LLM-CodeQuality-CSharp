using Application.Matrix.Exceptions;
using Application.Matrix.Resources;
using Domain.Interfaces;

namespace Application.Matrix;

public class MatrixString : IMatrixString
{
    private string[,] m;

    public MatrixString(int rows, int columns, string? value)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException(MatrixMessages.InvalidRowsColumns);

        if (value == null)
            throw new ArgumentException(MatrixMessages.InitialValueCannotBeNull);

        m = new string[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                m[i, j] = value;
            }
        }
    }

    public void Set(int row, int column, string? value)
    {
        if (row < 0 || row >= m.GetLength(0) || column < 0 || column >= m.GetLength(1))
            throw new MatrixException(MatrixMessages.IndexOutOfBounds);

        if (value == null)
            throw new MatrixException(MatrixMessages.ValueCannotBeNull);

        m[row, column] = value;
    }

    public string RowToString(int index, string? separator)
    {
        if (index < 0 || index >= m.GetLength(0))
            throw new MatrixException(MatrixMessages.RowIndexOutOfBounds);

        if (separator == null)
            throw new MatrixException(MatrixMessages.SeparatorCannotBeNull);

        string[] rowElements = new string[m.GetLength(1)];
        for (int j = 0; j < m.GetLength(1); j++)
        {
            rowElements[j] = m[index, j];
        }

        return string.Join(separator, rowElements);
    }
}
