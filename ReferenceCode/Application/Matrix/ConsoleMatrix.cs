using Application.Matrix.Exceptions;
using Application.Matrix.Utils;
using Application.Matrix.Resources;
using Domain.Interfaces;
using Domain.Enums;

namespace Application.Matrix;

public class ConsoleMatrix : IConsole
{
    private IMatrixString? _matrix;
    private readonly IMatrixStringFactory _matrixFactory;

    public ConsoleMatrix(IMatrixStringFactory matrixFactory)
    {
        _matrixFactory = matrixFactory;
    }

    public void Execute()
    {
        Console.WriteLine(MatrixMessages.Title);

        Console.Write(MatrixMessages.EnterRows);
        if (!int.TryParse(Console.ReadLine(), out int rows))
        {
            Console.WriteLine(MatrixMessages.InvalidRows);
            return;
        }

        Console.Write(MatrixMessages.EnterColumns);
        if (!int.TryParse(Console.ReadLine(), out int columns))
        {
            Console.WriteLine(MatrixMessages.InvalidColumns);
            return;
        }

        Console.Write(MatrixMessages.EnterInitialValue);
        string? value = Console.ReadLine().NormalizeInput();

        try
        {
            _matrix = _matrixFactory.Create(rows, columns, value);
            Console.WriteLine(string.Format(MatrixMessages.MatrixCreatedSuccess, rows, columns));

            Menu();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(string.Format(MatrixMessages.MatrixCreationError, ex.Message));
        }
    }

    private void Menu()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine(MatrixMessages.MenuTitle);
            Console.WriteLine(MatrixMessages.MenuOption1);
            Console.WriteLine(MatrixMessages.MenuOption2);
            Console.WriteLine(MatrixMessages.MenuOption3);
            Console.Write(MatrixMessages.ChooseOption);

            string option = Console.ReadLine().NormalizeInput() ?? string.Empty;

            try
            {
                if (int.TryParse(option, out int optionNumber) && Enum.IsDefined(typeof(MenuOption), optionNumber))
                {
                    var menuOption = (MenuOption)optionNumber;
                    switch (menuOption)
                    {
                        case MenuOption.Option1:
                            SetValue();
                            break;
                        case MenuOption.Option2:
                            ShowRow();
                            break;
                        case MenuOption.Option3:
                            running = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(MatrixMessages.InvalidOption);
                }
            }
            catch (MatrixException ex)
            {
                Console.WriteLine(string.Format(MatrixMessages.Error, ex.Message));
            }
        }
    }

    private void SetValue()
    {
        Console.Write(MatrixMessages.EnterRowIndex);
        if (!int.TryParse(Console.ReadLine(), out int row))
        {
            Console.WriteLine(MatrixMessages.InvalidIndex);
            return;
        }

        Console.Write(MatrixMessages.EnterColumnIndex);
        if (!int.TryParse(Console.ReadLine(), out int column))
        {
            Console.WriteLine(MatrixMessages.InvalidIndex);
            return;
        }

        Console.Write(string.Format(MatrixMessages.EnterValueForPosition, row, column));
        string? value = Console.ReadLine().NormalizeInput();

        _matrix?.Set(row, column, value);
        Console.WriteLine(MatrixMessages.ValueSetSuccess);
    }

    private void ShowRow()
    {
        Console.Write(MatrixMessages.EnterRowIndex);
        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine(MatrixMessages.InvalidIndex);
            return;
        }

        Console.Write(MatrixMessages.EnterSeparator);
        string? separator = Console.ReadLine().NormalizeInput();

        string result = _matrix?.RowToString(index, separator) ?? string.Empty;
        Console.WriteLine(string.Format(MatrixMessages.RowResult, index, result));
    }
}
