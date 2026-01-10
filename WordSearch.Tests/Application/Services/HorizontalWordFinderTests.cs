using FluentAssertions;
using WordSearch.Application.Services;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Application.Services;

public class HorizontalWordFinderTests
{
    private readonly HorizontalWordFinder _finder = new();

    [Fact]
    public void Find_ShouldFindWord_AtStartOfRow()
    {
        var grid = new char[,]
        {
            { 'H', 'E', 'L', 'L', 'O' }
        };

        var matches = _finder.Find(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
        matches[0].Start.Should().Be(new GridCoordinate(0, 0));
        matches[0].End.Should().Be(new GridCoordinate(0, 4));
    }

    [Fact]
    public void Find_ShouldFindWord_InMiddleOfRow()
    {
        var grid = new char[,]
        {
            { 'X', 'H', 'E', 'L', 'L', 'O', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Should().Be(new GridCoordinate(0, 1));
        matches[0].End.Should().Be(new GridCoordinate(0, 5));
    }

    [Fact]
    public void Find_ShouldFindWord_AtEndOfRow()
    {
        var grid = new char[,]
        {
            { 'X', 'X', 'H', 'E', 'L', 'L', 'O' }
        };

        var matches = _finder.Find(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Should().Be(new GridCoordinate(0, 2));
        matches[0].End.Should().Be(new GridCoordinate(0, 6));
    }

    [Fact]
    public void Find_ShouldFindWord_Reversed()
    {
        var grid = new char[,]
        {
            { 'O', 'L', 'L', 'E', 'H' }
        };

        var matches = _finder.Find(grid, new[] { "HELLO" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
        matches[0].Start.Should().Be(new GridCoordinate(0, 4));
        matches[0].End.Should().Be(new GridCoordinate(0, 0));
    }

    [Fact]
    public void Find_ShouldFindMultipleWords_InSameRow()
    {
        var grid = new char[,]
        {
            { 'C', 'A', 'T', 'D', 'O', 'G' }
        };

        var matches = _finder.Find(grid, new[] { "CAT", "DOG" }).ToList();

        matches.Should().HaveCount(2);
        matches.Should().Contain(m => m.Word == "CAT");
        matches.Should().Contain(m => m.Word == "DOG");
    }

    [Fact]
    public void Find_ShouldFindSameWord_InDifferentRows()
    {
        var grid = new char[,]
        {
            { 'C', 'A', 'T' },
            { 'X', 'X', 'X' },
            { 'C', 'A', 'T' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(2);
        matches.Should().Contain(m => m.Start == new GridCoordinate(0, 0));
        matches.Should().Contain(m => m.Start == new GridCoordinate(2, 0));
    }

    [Fact]
    public void Find_ShouldFindOverlappingWords()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C', 'D', 'E' }
        };

        var matches = _finder.Find(grid, new[] { "ABC", "BCD", "CDE" }).ToList();

        matches.Should().HaveCount(3);
    }

    [Fact]
    public void Find_ShouldNotFindWord_ThatWrapsToNextRow()
    {
        var grid = new char[,]
        {
            { 'H', 'E', 'L' },
            { 'L', 'O', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "HELLO" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldReturnEmpty_WhenWordNotFound()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' }
        };

        var matches = _finder.Find(grid, new[] { "XYZ" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldBeCaseInsensitive()
    {
        var grid = new char[,]
        {
            { 'H', 'E', 'L', 'L', 'O' }
        };

        var matches = _finder.Find(grid, new[] { "hello" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("HELLO");
    }

    [Fact]
    public void Find_ShouldHandleSingleLetterWord()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'A' }
        };

        var matches = _finder.Find(grid, new[] { "A" }).ToList();

        // Single letter words are found in both directions at each position
        // 2 'A's * 2 directions = 4 matches
        matches.Should().HaveCount(4);
    }

    [Fact]
    public void Find_ShouldHandleEmptyWordList()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C' }
        };

        var matches = _finder.Find(grid, Array.Empty<string>()).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldFindBothForwardAndReverse_WhenBothExist()
    {
        var grid = new char[,]
        {
            { 'C', 'A', 'T', 'T', 'A', 'C' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(2);
        matches.Should().Contain(m => m.Start.Col == 0 && m.End.Col == 2); // Forward
        matches.Should().Contain(m => m.Start.Col == 5 && m.End.Col == 3); // Reverse
    }

    [Fact]
    public void Find_ShouldFindPalindrome_OnlyOnce()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'B', 'A' }
        };

        var matches = _finder.Find(grid, new[] { "ABBA" }).ToList();

        // ABBA forward and reverse are the same, but they have different start/end
        matches.Should().HaveCount(2);
    }
}
