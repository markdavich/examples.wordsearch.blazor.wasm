using System.Text.RegularExpressions;
using WordSearchSolver.Application.Interfaces;
using WordSearchSolver.Domain.Entities;
using WordSearchSolver.Domain.Enums;
using WordSearchSolver.Domain.ValueObjects;

using Match = WordSearchSolver.Domain.Entities.Match;

namespace WordSearchSolver.Application.Services;

/// <summary>
/// Finds words diagonally (both directions).
/// </summary>
public sealed class DiagonalWordFinder : IDirectionalFinder
{
    public IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        // Extract diagonals going down-right (from left)
        var fromLeftDiagonals = ExtractDiagonals(grid, DiagonalDirection.FromLeft);

        // Extract diagonals going down-left (from right)
        var fromRightDiagonals = ExtractDiagonals(grid, DiagonalDirection.FromRight);

        foreach (var word in words)
        {
            var upperWord = word.ToUpperInvariant();
            var reversedWord = new string(upperWord.Reverse().ToArray());

            // Search in from-left diagonals
            foreach (var match in FindInDiagonals(fromLeftDiagonals, upperWord, DiagonalDirection.FromLeft, rows, cols, isForward: true))
                yield return match;

            foreach (var match in FindInDiagonals(fromLeftDiagonals, reversedWord, DiagonalDirection.FromLeft, rows, cols, isForward: false, originalWord: upperWord))
                yield return match;

            // Search in from-right diagonals
            foreach (var match in FindInDiagonals(fromRightDiagonals, upperWord, DiagonalDirection.FromRight, rows, cols, isForward: true))
                yield return match;

            foreach (var match in FindInDiagonals(fromRightDiagonals, reversedWord, DiagonalDirection.FromRight, rows, cols, isForward: false, originalWord: upperWord))
                yield return match;
        }
    }

    private static List<List<(char Char, int Row, int Col)>> ExtractDiagonals(char[,] grid, DiagonalDirection direction)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);
        var diagonalCount = rows + cols - 1;
        var diagonals = new List<List<(char, int, int)>>(diagonalCount);

        for (var i = 0; i < diagonalCount; i++)
        {
            diagonals.Add(new List<(char, int, int)>());
        }

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                int diagonalIndex;

                if (direction == DiagonalDirection.FromLeft)
                {
                    // Diagonals going down-right: same (row - col) value
                    diagonalIndex = row - col + (cols - 1);
                }
                else
                {
                    // Diagonals going down-left: same (row + col) value
                    diagonalIndex = row + col;
                }

                diagonals[diagonalIndex].Add((grid[row, col], row, col));
            }
        }

        return diagonals;
    }

    private static IEnumerable<Match> FindInDiagonals(
        List<List<(char Char, int Row, int Col)>> diagonals,
        string searchWord,
        DiagonalDirection direction,
        int gridRows,
        int gridCols,
        bool isForward,
        string? originalWord = null)
    {
        foreach (var diagonal in diagonals)
        {
            if (diagonal.Count < searchWord.Length)
                continue;

            var diagonalString = new string(diagonal.Select(d => d.Char).ToArray());
            var pattern = new Regex(Regex.Escape(searchWord), RegexOptions.IgnoreCase);
            var matches = pattern.Matches(diagonalString);

            foreach (System.Text.RegularExpressions.Match regexMatch in matches)
            {
                var startIndex = regexMatch.Index;
                var endIndex = startIndex + searchWord.Length - 1;

                var word = originalWord ?? searchWord;
                var (_, startRow, startCol) = diagonal[startIndex];
                var (_, endRow, endCol) = diagonal[endIndex];

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
}
