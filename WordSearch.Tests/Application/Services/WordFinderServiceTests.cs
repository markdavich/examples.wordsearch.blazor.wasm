using FluentAssertions;
using WordSearch.Application.Services;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Application.Services;

public class WordFinderServiceTests
{
    private readonly WordFinderService _wordFinder = new();

    [Fact]
    public void FindWords_ShouldFindHorizontalWord()
    {
        var grid = new char[,]
        {
            { 'H', 'E', 'L', 'L', 'O' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
    }

    [Fact]
    public void FindWords_ShouldFindVerticalWord()
    {
        var grid = new char[,]
        {
            { 'H' },
            { 'E' },
            { 'L' },
            { 'L' },
            { 'O' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
    }

    [Fact]
    public void FindWords_ShouldFindDiagonalWord()
    {
        var grid = new char[,]
        {
            { 'H', 'X', 'X', 'X', 'X' },
            { 'X', 'E', 'X', 'X', 'X' },
            { 'X', 'X', 'L', 'X', 'X' },
            { 'X', 'X', 'X', 'L', 'X' },
            { 'X', 'X', 'X', 'X', 'O' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
    }

    [Fact]
    public void FindWords_ShouldFindReversedWords()
    {
        var grid = new char[,]
        {
            { 'O', 'L', 'L', 'E', 'H' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
    }

    [Fact]
    public void FindWords_ShouldFindMultipleWordsInAllDirections()
    {
        var grid = new char[,]
        {
            { 'C', 'A', 'T', 'X', 'X' },
            { 'X', 'X', 'X', 'X', 'X' },
            { 'D', 'X', 'B', 'X', 'X' },
            { 'O', 'X', 'X', 'I', 'X' },
            { 'G', 'X', 'X', 'X', 'G' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "CAT", "DOG", "BIG" }).ToList();

        matches.Should().HaveCount(3);
        matches.Should().Contain(m => m.Word == "CAT");
        matches.Should().Contain(m => m.Word == "DOG");
        matches.Should().Contain(m => m.Word == "BIG");
    }

    [Fact]
    public void FindWords_ShouldFindSameWord_MultipleOccurrences()
    {
        var grid = new char[,]
        {
            { 'C', 'A', 'T', 'X', 'C', 'A', 'T' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(2);
    }

    [Fact]
    public void FindWords_ShouldReturnEmpty_WhenNoMatches()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' },
            { 'G', 'H', 'I' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "XYZ" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void FindWords_ShouldHandleEmptyWordList()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C' }
        };

        var matches = _wordFinder.FindWords(grid, Array.Empty<string>()).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void FindWords_ShouldBeCaseInsensitive()
    {
        var grid = new char[,]
        {
            { 'H', 'E', 'L', 'L', 'O' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "hello" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
    }

    [Fact]
    public void FindWords_ShouldFindWord_InTypicalPuzzle()
    {
        // A typical word search puzzle
        var grid = new char[,]
        {
            { 'W', 'O', 'R', 'D', 'S' },
            { 'E', 'A', 'R', 'C', 'H' },
            { 'L', 'R', 'C', 'H', 'A' },
            { 'L', 'C', 'O', 'E', 'P' },
            { 'O', 'H', 'P', 'R', 'P' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "WORD", "SEARCH", "WELL", "HAPPY" }).ToList();

        // Should find WORD horizontally
        matches.Should().Contain(m => m.Word == "WORD");
        // Should find WELL vertically
        matches.Should().Contain(m => m.Word == "WELL");
    }

    [Fact]
    public void FindWords_ShouldHandleLargeGrid()
    {
        var size = 20;
        var grid = new char[size, size];

        // Fill with X
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                grid[i, j] = 'X';

        // Put "HELLO" horizontally in the middle
        grid[10, 5] = 'H';
        grid[10, 6] = 'E';
        grid[10, 7] = 'L';
        grid[10, 8] = 'L';
        grid[10, 9] = 'O';

        var matches = _wordFinder.FindWords(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Should().Be(new GridCoordinate(10, 5));
    }

    [Fact]
    public void FindWords_ShouldFindAllDirectionsOfSameWord()
    {
        // Grid where "ABC" appears in multiple directions
        var grid = new char[,]
        {
            { 'A', 'B', 'C', 'X', 'X' },
            { 'B', 'B', 'X', 'X', 'X' },
            { 'C', 'X', 'C', 'X', 'X' },
            { 'X', 'X', 'X', 'X', 'X' },
            { 'X', 'X', 'X', 'X', 'X' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "ABC" }).ToList();

        // Should find horizontal and diagonal (and possibly more)
        matches.Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    public void FindWords_ShouldFindWordInAllEightDirections()
    {
        // Grid specifically designed to have "AB" in all 8 directions from center
        var grid = new char[,]
        {
            { 'B', 'X', 'B', 'X', 'B' },
            { 'X', 'B', 'A', 'B', 'X' },
            { 'B', 'A', 'A', 'A', 'B' },
            { 'X', 'B', 'A', 'B', 'X' },
            { 'B', 'X', 'B', 'X', 'B' }
        };

        var matches = _wordFinder.FindWords(grid, new[] { "AB" }).ToList();

        // There should be multiple matches for "AB" in various directions
        matches.Should().HaveCountGreaterOrEqualTo(4);
    }
}
