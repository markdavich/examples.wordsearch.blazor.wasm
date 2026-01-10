using System.Text.RegularExpressions;
using WordSearch.Application.Interfaces;
using WordSearch.Domain.Entities;
using WordSearch.Domain.ValueObjects;

using Match = WordSearch.Domain.Entities.Match;

namespace WordSearch.Application.Services;

/// <summary>
/// Finds words horizontally (left-to-right and right-to-left).
/// </summary>
public sealed class HorizontalWordFinder : IDirectionalFinder
{
    public IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words)
    {
        var columnCount = grid.GetLength(1);
        var data = MatrixService.Flatten(grid);

        foreach (var word in words)
        {
            var upperWord = word.ToUpperInvariant();

            // Find forward matches
            foreach (var match in FindMatches(data, upperWord, columnCount, isForward: true))
            {
                yield return match;
            }

            // Find reverse matches
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
            var (startRow, startCol) = MatrixService.IndexToCoordinates(index, columnCount);
            var endCol = startCol + searchWord.Length - 1;

            // Check if match wraps to next line (invalid)
            if (endCol >= columnCount)
                continue;

            var word = originalWord ?? searchWord;
            var start = new GridCoordinate(startRow, startCol);
            var end = new GridCoordinate(startRow, endCol);

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
