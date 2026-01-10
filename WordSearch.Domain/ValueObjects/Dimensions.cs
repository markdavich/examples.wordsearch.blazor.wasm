namespace WordSearch.Domain.ValueObjects;

/// <summary>
/// Represents the dimensions of a word search grid.
/// </summary>
public readonly record struct Dimensions(int Rows, int Columns)
{
    public override string ToString() => $"{Rows}x{Columns}";

    public static Dimensions Parse(string dimensions)
    {
        var parts = dimensions.ToLowerInvariant().Split('x');
        return new Dimensions(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static bool TryParse(string dimensions, out Dimensions result)
    {
        result = default;
        var parts = dimensions.ToLowerInvariant().Split('x');
        if (parts.Length != 2) return false;
        if (!int.TryParse(parts[0], out var rows)) return false;
        if (!int.TryParse(parts[1], out var cols)) return false;
        result = new Dimensions(rows, cols);
        return true;
    }
}
