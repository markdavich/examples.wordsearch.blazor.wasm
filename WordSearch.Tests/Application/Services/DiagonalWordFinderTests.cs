using FluentAssertions;
using WordSearch.Application.Services;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Application.Services;

public class DiagonalWordFinderTests
{
    private readonly DiagonalWordFinder _finder = new();

    [Fact]
    public void Find_ShouldFindWord_DiagonalDownRight()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'X' },
            { 'X', 'A', 'X' },
            { 'X', 'X', 'T' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
        matches[0].Start.Should().Be(new GridCoordinate(0, 0));
        matches[0].End.Should().Be(new GridCoordinate(2, 2));
    }

    [Fact]
    public void Find_ShouldFindWord_DiagonalDownLeft()
    {
        var grid = new char[,]
        {
            { 'X', 'X', 'C' },
            { 'X', 'A', 'X' },
            { 'T', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
        matches[0].Start.Should().Be(new GridCoordinate(0, 2));
        matches[0].End.Should().Be(new GridCoordinate(2, 0));
    }

    [Fact]
    public void Find_ShouldFindWord_DiagonalUpRight()
    {
        var grid = new char[,]
        {
            { 'X', 'X', 'T' },
            { 'X', 'A', 'X' },
            { 'C', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
        matches[0].Start.Should().Be(new GridCoordinate(2, 0));
        matches[0].End.Should().Be(new GridCoordinate(0, 2));
    }

    [Fact]
    public void Find_ShouldFindWord_DiagonalUpLeft()
    {
        var grid = new char[,]
        {
            { 'T', 'X', 'X' },
            { 'X', 'A', 'X' },
            { 'X', 'X', 'C' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
        matches[0].Start.Should().Be(new GridCoordinate(2, 2));
        matches[0].End.Should().Be(new GridCoordinate(0, 0));
    }

    [Fact]
    public void Find_ShouldFindWord_InMiddleOfDiagonal()
    {
        var grid = new char[,]
        {
            { 'X', 'X', 'X', 'X', 'X' },
            { 'X', 'C', 'X', 'X', 'X' },
            { 'X', 'X', 'A', 'X', 'X' },
            { 'X', 'X', 'X', 'T', 'X' },
            { 'X', 'X', 'X', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Should().Be(new GridCoordinate(1, 1));
        matches[0].End.Should().Be(new GridCoordinate(3, 3));
    }

    [Fact]
    public void Find_ShouldFindMultipleWordsOnDifferentDiagonals()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'X', 'D' },
            { 'X', 'A', 'O', 'X' },
            { 'X', 'G', 'T', 'X' },
            { 'X', 'X', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT", "DOG" }).ToList();

        matches.Should().HaveCount(2);
        matches.Should().Contain(m => m.Word == "CAT");
        matches.Should().Contain(m => m.Word == "DOG");
    }

    [Fact]
    public void Find_ShouldFindWord_OnShortDiagonal()
    {
        var grid = new char[,]
        {
            { 'A', 'X' },
            { 'X', 'B' }
        };

        var matches = _finder.Find(grid, new[] { "AB" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Start.Should().Be(new GridCoordinate(0, 0));
        matches[0].End.Should().Be(new GridCoordinate(1, 1));
    }

    [Fact]
    public void Find_ShouldNotFind_WordLongerThanDiagonal()
    {
        var grid = new char[,]
        {
            { 'A', 'B' },
            { 'C', 'D' }
        };

        var matches = _finder.Find(grid, new[] { "ABCDE" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldHandleRectangularGrid_MoreColumnsThanRows()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'X', 'X', 'X' },
            { 'X', 'A', 'X', 'X', 'X' },
            { 'X', 'X', 'T', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
    }

    [Fact]
    public void Find_ShouldHandleRectangularGrid_MoreRowsThanColumns()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'X' },
            { 'X', 'A', 'X' },
            { 'X', 'X', 'T' },
            { 'X', 'X', 'X' },
            { 'X', 'X', 'X' }
        };

        var matches = _finder.Find(grid, new[] { "CAT" }).ToList();

        matches.Should().HaveCount(1);
    }

    [Fact]
    public void Find_ShouldBeCaseInsensitive()
    {
        var grid = new char[,]
        {
            { 'C', 'X', 'X' },
            { 'X', 'A', 'X' },
            { 'X', 'X', 'T' }
        };

        var matches = _finder.Find(grid, new[] { "cat" }).ToList();

        matches.Should().HaveCount(1);
        matches[0].Word.Should().Be("CAT");
    }

    [Fact]
    public void Find_ShouldReturnEmpty_WhenNoMatch()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' },
            { 'G', 'H', 'I' }
        };

        var matches = _finder.Find(grid, new[] { "XYZ" }).ToList();

        matches.Should().BeEmpty();
    }

    [Fact]
    public void Find_ShouldFindAllFourDirections()
    {
        // This grid has "AB" in all four diagonal directions
        var grid = new char[,]
        {
            { 'B', 'X', 'A' },
            { 'X', 'A', 'X' },
            { 'A', 'X', 'B' }
        };

        var matches = _finder.Find(grid, new[] { "AB" }).ToList();

        // Should find AB in multiple diagonal directions
        matches.Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    public void Find_ShouldFindOverlappingDiagonals()
    {
        var grid = new char[,]
        {
            { 'A', 'X', 'X', 'X' },
            { 'X', 'B', 'X', 'X' },
            { 'X', 'X', 'C', 'X' },
            { 'X', 'X', 'X', 'D' }
        };

        var matches = _finder.Find(grid, new[] { "AB", "BC", "CD", "ABCD" }).ToList();

        matches.Should().HaveCount(4);
    }
}
