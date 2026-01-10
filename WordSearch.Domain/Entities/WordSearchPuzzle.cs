using WordSearch.Domain.ValueObjects;

namespace WordSearch.Domain.Entities;

/// <summary>
/// Represents a word search puzzle with a letter grid and words to find.
/// </summary>
public sealed class WordSearchPuzzle
{
    public Dimensions Dimensions { get; }
    public char[,] Letters { get; }
    public IReadOnlyList<string> Words { get; }

    private WordSearchPuzzle(Dimensions dimensions, char[,] letters, IReadOnlyList<string> words)
    {
        Dimensions = dimensions;
        Letters = letters;
        Words = words;
    }

    /// <summary>
    /// Parses a word search puzzle from file content.
    /// Expected format:
    /// - First line: dimensions (e.g., "5x5")
    /// - Next N lines: letter grid (space-separated or continuous)
    /// - Remaining lines: words to find
    /// </summary>
    public static WordSearchPuzzle Parse(string fileContent)
    {
        var lines = GetLines(fileContent);
        if (lines.Count < 2)
            throw new ArgumentException("File must contain at least dimensions and one grid row.");

        var dimensions = ParseDimensions(lines[0]);
        var letters = ParseLetters(lines, dimensions.Rows);
        var words = ParseWords(lines, dimensions.Rows);

        return new WordSearchPuzzle(dimensions, letters, words);
    }

    private static List<string> GetLines(string content)
    {
        // Handle all line ending types: CRLF, CR, LF
        return content
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line))
            .ToList();
    }

    private static Dimensions ParseDimensions(string line)
    {
        if (!Dimensions.TryParse(line, out var dimensions))
            throw new ArgumentException($"Invalid dimensions format: '{line}'. Expected format: 'ROWSxCOLUMNS' (e.g., '10x10').");
        return dimensions;
    }

    private static char[,] ParseLetters(List<string> lines, int rows)
    {
        if (lines.Count < rows + 1)
            throw new ArgumentException($"Expected {rows} grid rows but found fewer lines.");

        // Determine columns from first grid row
        var firstRow = lines[1].Replace(" ", "");
        var columns = firstRow.Length;

        var letters = new char[rows, columns];

        for (var row = 0; row < rows; row++)
        {
            var rowLetters = lines[row + 1].Replace(" ", "").ToUpperInvariant();
            if (rowLetters.Length != columns)
                throw new ArgumentException($"Row {row + 1} has {rowLetters.Length} letters but expected {columns}.");

            for (var col = 0; col < columns; col++)
            {
                letters[row, col] = rowLetters[col];
            }
        }

        return letters;
    }

    private static List<string> ParseWords(List<string> lines, int gridRows)
    {
        var words = new List<string>();

        for (var i = gridRows + 1; i < lines.Count; i++)
        {
            var word = lines[i].Trim().ToUpperInvariant();
            if (!string.IsNullOrEmpty(word))
            {
                words.Add(word);
            }
        }

        return words;
    }
}
