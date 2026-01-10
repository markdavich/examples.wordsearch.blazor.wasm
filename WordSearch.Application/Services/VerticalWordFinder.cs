using System.Text.RegularExpressions;
using WordSearch.Application.Interfaces;
using WordSearch.Domain.Entities;
using WordSearch.Domain.ValueObjects;

using Match = WordSearch.Domain.Entities.Match;

namespace WordSearch.Application.Services;

/// <summary>
/// Finds words vertically (top-to-bottom and bottom-to-top).
/// Uses matrix transpose to convert to horizontal searching.
/// </summary>
public sealed class VerticalWordFinder : IDirectionalFinder
{
    public IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words)
    {
        // Transpose the grid (columns become rows)
        var transposed = MatrixService.Transpose(grid);
        var columnCount = transposed.GetLength(1);
        var data = MatrixService.Flatten(transposed);

        foreach (var word in words)
        {
            var upperWord = word.ToUpperInvariant();

            // Find forward matches (top-to-bottom in original)
            foreach (var match in FindMatches(data, upperWord, columnCount, isForward: true))
            {
                yield return match;
            }

            // Find reverse matches (bottom-to-top in original)
            var reversedWord = new string(upperWord.Reverse().ToArray());
            foreach (var match in FindMatches(data, reversedWord, columnCount, isForward: false, originalWord: upperWord))
            {
                yield return match;
            }
        }
    }

    private static IEnumerable<Match> FindMatches(
        string data,
        string searchWord,
        int columnCount,
        bool isForward,
        string? originalWord = null)
    {
        var pattern = new Regex(Regex.Escape(searchWord), RegexOptions.IgnoreCase);
        var matches = pattern.Matches(data);

        foreach (System.Text.RegularExpressions.Match regexMatch in matches)
        {
            var index = regexMatch.Index;
            var (transposedRow, transposedCol) = MatrixService.IndexToCoordinates(index, columnCount);
            var endTransposedCol = transposedCol + searchWord.Length - 1;

            // Check if match wraps to next line (invalid)
            if (endTransposedCol >= columnCount)
                continue;

            // Convert transposed coordinates back to original grid
            // In transposed: row = original col, col = original row
            var word = originalWord ?? searchWord;
            var startRow = transposedCol;
            var startCol = transposedRow;
            var endRow = endTransposedCol;
            var endCol = transposedRow;

            var start = new GridCoordinate(startRow, startCol);
            var end = new GridCoordinate(endRow, endCol);

            // For reverse matches, swap start and end
            if (!isForward)
            {
                yield return new Match(word, end, start);
            }
            else
            {
                yield return new Match(word, start, end);
            }
        }
    }
}
