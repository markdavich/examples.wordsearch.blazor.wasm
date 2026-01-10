using WordSearch.Domain.Enums;

namespace WordSearch.Application.Services;

/// <summary>
/// Service for matrix operations (transpose, padding).
/// </summary>
public static class MatrixService
{
    /// <summary>
    /// Transposes a 2D array (swaps rows and columns).
    /// </summary>
    public static T[,] Transpose<T>(T[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var result = new T[cols, rows];

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                result[col, row] = matrix[row, col];
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the shape of a matrix.
    /// </summary>
    public static MatrixShape GetShape(int rows, int columns)
    {
        if (rows > columns) return MatrixShape.Tall;
        if (columns > rows) return MatrixShape.Wide;
        return MatrixShape.Square;
    }

    /// <summary>
    /// Flattens a 2D char array to a single string.
    /// </summary>
    public static string Flatten(char[,] grid)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);
        var chars = new char[rows * cols];

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                chars[row * cols + col] = grid[row, col];
            }
        }

        return new string(chars);
    }

    /// <summary>
    /// Converts a flat index to row/column coordinates.
    /// </summary>
    public static (int Row, int Col) IndexToCoordinates(int index, int columnCount)
    {
        var row = index / columnCount;
        var col = index % columnCount;
        return (row, col);
    }
}
