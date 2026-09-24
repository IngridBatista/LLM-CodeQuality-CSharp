namespace Domain.Interfaces;

public interface IMatrixString
{
    void Set(int row, int column, string? value);
    string RowToString(int index, string? separator);
}
