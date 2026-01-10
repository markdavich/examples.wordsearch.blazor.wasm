using WordSearch.Application.Interfaces;
using WordSearch.Domain.Entities;

namespace WordSearch.Application.Services;

/// <summary>
/// Aggregates word finding across all directions (horizontal, vertical, diagonal).
/// </summary>
public sealed class WordFinderService : IWordFinder
{
    private readonly HorizontalWordFinder _horizontalFinder;
    private readonly VerticalWordFinder _verticalFinder;
    private readonly DiagonalWordFinder _diagonalFinder;

    public WordFinderService()
    {
        _horizontalFinder = new HorizontalWordFinder();
        _verticalFinder = new VerticalWordFinder();
        _diagonalFinder = new DiagonalWordFinder();
    }

    public IEnumerable<Match> FindWords(char[,] grid, IEnumerable<string> words)
    {
        var wordList = words.ToList();

        // Find in all directions
        var horizontal = _horizontalFinder.Find(grid, wordList);
        var vertical = _verticalFinder.Find(grid, wordList);
        var diagonal = _diagonalFinder.Find(grid, wordList);

        // Combine all matches
        return horizontal.Concat(vertical).Concat(diagonal);
    }
}
