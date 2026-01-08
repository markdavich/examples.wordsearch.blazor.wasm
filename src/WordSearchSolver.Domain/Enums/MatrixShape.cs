namespace WordSearchSolver.Domain.Enums;

/// <summary>
/// Shape of a matrix (tall, wide, or square).
/// </summary>
public enum MatrixShape
{
    /// <summary>
    /// More rows than columns.
    /// </summary>
    Tall,

    /// <summary>
    /// More columns than rows.
    /// </summary>
    Wide,

    /// <summary>
    /// Equal rows and columns.
    /// </summary>
    Square
}
