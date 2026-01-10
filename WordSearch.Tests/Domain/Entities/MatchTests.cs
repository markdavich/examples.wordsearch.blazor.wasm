using FluentAssertions;
using WordSearch.Domain.Entities;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Domain.Entities;

public class MatchTests
{
    [Fact]
    public void Constructor_ShouldSetAllProperties()
    {
        var start = new GridCoordinate(0, 0);
        var end = new GridCoordinate(0, 4);

        var match = new Match("HELLO", start, end);

        match.Word.Should().Be("HELLO");
        match.Start.Should().Be(start);
        match.End.Should().Be(end);
    }

    [Fact]
    public void GetPath_ShouldReturnCorrectCells_ForHorizontalWord()
    {
        var match = new Match("HELLO", new GridCoordinate(0, 0), new GridCoordinate(0, 4));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(5);
        path[0].Should().Be(new GridCoordinate(0, 0));
        path[1].Should().Be(new GridCoordinate(0, 1));
        path[2].Should().Be(new GridCoordinate(0, 2));
        path[3].Should().Be(new GridCoordinate(0, 3));
        path[4].Should().Be(new GridCoordinate(0, 4));
    }

    [Fact]
    public void GetPath_ShouldReturnCorrectCells_ForVerticalWord()
    {
        var match = new Match("CAT", new GridCoordinate(0, 0), new GridCoordinate(2, 0));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(3);
        path[0].Should().Be(new GridCoordinate(0, 0));
        path[1].Should().Be(new GridCoordinate(1, 0));
        path[2].Should().Be(new GridCoordinate(2, 0));
    }

    [Fact]
    public void GetPath_ShouldReturnCorrectCells_ForDiagonalDownRight()
    {
        var match = new Match("DOG", new GridCoordinate(0, 0), new GridCoordinate(2, 2));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(3);
        path[0].Should().Be(new GridCoordinate(0, 0));
        path[1].Should().Be(new GridCoordinate(1, 1));
        path[2].Should().Be(new GridCoordinate(2, 2));
    }

    [Fact]
    public void GetPath_ShouldReturnCorrectCells_ForDiagonalDownLeft()
    {
        var match = new Match("DOG", new GridCoordinate(0, 2), new GridCoordinate(2, 0));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(3);
        path[0].Should().Be(new GridCoordinate(0, 2));
        path[1].Should().Be(new GridCoordinate(1, 1));
        path[2].Should().Be(new GridCoordinate(2, 0));
    }

    [Fact]
    public void GetPath_ShouldReturnCorrectCells_ForReversedHorizontal()
    {
        var match = new Match("OLLEH", new GridCoordinate(0, 4), new GridCoordinate(0, 0));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(5);
        path[0].Should().Be(new GridCoordinate(0, 4));
        path[1].Should().Be(new GridCoordinate(0, 3));
        path[2].Should().Be(new GridCoordinate(0, 2));
        path[3].Should().Be(new GridCoordinate(0, 1));
        path[4].Should().Be(new GridCoordinate(0, 0));
    }

    [Fact]
    public void GetPath_ShouldReturnCorrectCells_ForReversedVertical()
    {
        var match = new Match("TAC", new GridCoordinate(2, 0), new GridCoordinate(0, 0));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(3);
        path[0].Should().Be(new GridCoordinate(2, 0));
        path[1].Should().Be(new GridCoordinate(1, 0));
        path[2].Should().Be(new GridCoordinate(0, 0));
    }

    [Fact]
    public void GetPath_ShouldReturnSingleCell_ForSingleCharacterWord()
    {
        var match = new Match("A", new GridCoordinate(5, 5), new GridCoordinate(5, 5));

        var path = match.GetPath().ToList();

        path.Should().HaveCount(1);
        path[0].Should().Be(new GridCoordinate(5, 5));
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        var match = new Match("HELLO", new GridCoordinate(0, 0), new GridCoordinate(0, 4));

        match.ToString().Should().Be("HELLO 0:0 0:4");
    }

    [Fact]
    public void Parse_ShouldCreateCorrectMatch()
    {
        var match = Match.Parse("HELLO 0:0 0:4");

        match.Word.Should().Be("HELLO");
        match.Start.Should().Be(new GridCoordinate(0, 0));
        match.End.Should().Be(new GridCoordinate(0, 4));
    }

    [Fact]
    public void Parse_ShouldHandleDifferentCoordinates()
    {
        var match = Match.Parse("TEST 5:10 8:13");

        match.Word.Should().Be("TEST");
        match.Start.Should().Be(new GridCoordinate(5, 10));
        match.End.Should().Be(new GridCoordinate(8, 13));
    }

    [Fact]
    public void RoundTrip_ParseAndToString_ShouldPreserveValues()
    {
        var original = new Match("WORD", new GridCoordinate(3, 7), new GridCoordinate(3, 10));
        var parsed = Match.Parse(original.ToString());

        parsed.Word.Should().Be(original.Word);
        parsed.Start.Should().Be(original.Start);
        parsed.End.Should().Be(original.End);
    }
}
