using WordSearch.Domain.ValueObjects;

namespace WordSearch.Domain.Entities;

/// <summary>
/// Represents a found word in the grid with its start and end coordinates.
/// </summary>
public sealed class Match
{
    public string Word { get; }
    public GridCoordinate Start { get; }
    public GridCoordinate End { get; }

    public Match(string word, GridCoordinate start, GridCoordinate end)
    {
        Word = word;
        Start = start;
        End = end;
    }

    /// <summary>
    /// Gets all cells along the path from Start to End.
    /// </summary>
    public IEnumerable<GridCoordinate> GetPath()
    {
        var rowStep = Math.Sign(End.Row - Start.Row);
        var colStep = Math.Sign(End.Col - Start.Col);
        var steps = Math.Max(Math.Abs(End.Row - Start.Row), Math.Abs(End.Col - Start.Col));

        for (var i = 0; i <= steps; i++)
        {
            yield return new GridCoordinate(Start.Row + i * rowStep, Start.Col + i * colStep);
        }
    }

    public override string ToString() => $"{Word} {Start} {End}";

    public static Match Parse(string matchString)
    {
        var parts = matchString.Split(' ');
        var word = parts[0];
        var start = GridCoordinate.Parse(parts[1]);
        var end = GridCoordinate.Parse(parts[2]);
        return new Match(word, start, end);
    }
}
