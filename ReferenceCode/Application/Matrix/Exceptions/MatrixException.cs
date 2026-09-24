namespace Application.Matrix.Exceptions;

public class MatrixException : Exception
{
    public MatrixException() : base()
    {
    }

    public MatrixException(string message) : base(message)
    {
    }

    public MatrixException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
