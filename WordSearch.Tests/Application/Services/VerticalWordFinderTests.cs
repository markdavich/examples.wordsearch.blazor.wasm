using FluentAssertions;
using WordSearch.Application.Services;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Application.Services;

public class VerticalWordFinderTests
{
    private readonly VerticalWordFinder _finder = new();

    [Fact]
    public void Find_ShouldFindWord_TopToBottom()
    {
        var grid = new char[,]
        {
            { 'C' },
            { 'A' },
            { 'T' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
        matches[0].Start.Should().Be(new GridCoordinate(0, 0));
        matches[0].End.Should().Be(new GridCoordinate(2, 0));
    }

    [Fact]
    public void Find_ShouldFindWord_BottomToTop()
    {
        var grid = new char[,]
        {
            { 'T' },
            { 'A' },
            { 'C' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
        matches[0].Start.Should().Be(new GridCoordinate(2, 0));
        matches[0].End.Should().Be(new GridCoordinate(0, 0));
    }

    [Fact]
    public void Find_ShouldFindWord_InMiddleOfColumn()
    {
        var grid = new char[,]
        {
            { 'X' },
            { 'C' },
            { 'A' },
            { 'T' },
            { 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Should().Be(new GridCoordinate(1, 0));
        matches[0].End.Should().Be(new GridCoordinate(3, 0));
    }

    [Fact]
    public void Find_ShouldFindWord_InDifferentColumns()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'D' },
            { 'A', 'X', 'O' },
            { 'T', 'X', 'G' }
        };

        var matches = _finder.Find(grid, new[] { "CAT", "DOG" }).ToList();

        matches.Should().HaveCount(2);
        var catMatch = matches.First(m => m.Word == "CAT");
        var dogMatch = matches.First(m => m.Word == "DOG");

        catMatch.Start.Col.Should().Be(0);
        dogMatch.Start.Col.Should().Be(2);
    }

    [Fact]
    public void Find_ShouldFindSameWord_InMultipleColumns()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'C' },
            { 'A', 'X', 'A' },
            { 'T', 'X', 'T' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(2);
        matches.Should().Contain(m => m.Start.Col == 0);
        matches.Should().Contain(m => m.Start.Col == 2);
    }

    [Fact]
    public void Find_ShouldNotFindWord_ThatSpansAcrossColumns()
    {
        var grid = new char[,]
        {
            { 'C', 'A' },
            { 'T', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldReturnEmpty_WhenWordNotFound()
    {
        var grid = new char[,]
        {
            { 'A' },
            { 'B' },
            { 'C' }
        };

        var matches = _finder.Find(grid, new[] { "XYZ" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldBeCaseInsensitive()
    {
        var grid = new char[,]
        {
            { 'C' },
            { 'A' },
            { 'T' }
        };

        var matches = _finder.Find(grid, new[] { "cat" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
    }

    [Fact]
    public void Find_ShouldHandleWideGrid()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'X', 'X', 'X' },
            { 'A', 'X', 'X', 'X', 'X' },
            { 'T', 'X', 'X', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Col.Should().Be(0);
    }

    [Fact]
    public void Find_ShouldHandleTallGrid()
    {
        var grid = new char[,]
        {
            { 'X' },
            { 'X' },
            { 'C' },
            { 'A' },
            { 'T' },
            { 'X' },
            { 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Row.Should().Be(2);
        matches[0].End.Row.Should().Be(4);
    }

    [Fact]
    public void Find_ShouldFindBothDirections_WhenBothExist()
    {
        var grid = new char[,]
        {
            { 'C' },
            { 'A' },
            { 'T' },
            { 'T' },
            { 'A' },
            { 'C' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(2);
        matches.Should().Contain(m => m.Start.Row == 0 && m.End.Row == 2); // Top-to-bottom
        matches.Should().Contain(m => m.Start.Row == 5 && m.End.Row == 3); // Bottom-to-top
    }
}
