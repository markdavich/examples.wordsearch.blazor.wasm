namespace WordSearch.Domain.ValueObjects;

/// <summary>
/// Represents a position in the word search grid.
/// </summary>
public readonly record struct GridCoordinate(int Row, int Col)
{
    public override string ToString() => $"{Row}:{Col}";

    public static GridCoordinate Parse(string coordinate)
    {
        var parts = coordinate.Split(':');
        return new GridCoordinate(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
