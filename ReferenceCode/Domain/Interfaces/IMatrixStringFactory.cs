namespace Domain.Interfaces;

public interface IMatrixStringFactory
{
    IMatrixString Create(int rows, int columns, string? value);
}
