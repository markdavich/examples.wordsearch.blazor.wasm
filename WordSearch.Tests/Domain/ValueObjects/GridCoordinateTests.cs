using FluentAssertions;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Domain.ValueObjects;

public class GridCoordinateTests
{
    [Fact]
    public void Constructor_ShouldSetRowAndCol()
    {
        var coordinate = new GridCoordinate(5, 10);

        coordinate.Row.Should().Be(5);
        coordinate.Col.Should().Be(10);
    }

    [Fact]
    public void Constructor_ShouldHandleZeroValues()
    {
        var coordinate = new GridCoordinate(0, 0);

        coordinate.Row.Should().Be(0);
        coordinate.Col.Should().Be(0);
    }

    [Fact]
    public void Constructor_ShouldHandleNegativeValues()
    {
        var coordinate = new GridCoordinate(-1, -5);

        coordinate.Row.Should().Be(-1);
        coordinate.Col.Should().Be(-5);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        var coordinate = new GridCoordinate(3, 7);

        coordinate.ToString().Should().Be("3:7");
    }

    [Fact]
    public void ToString_ShouldHandleZeroValues()
    {
        var coordinate = new GridCoordinate(0, 0);

        coordinate.ToString().Should().Be("0:0");
    }

    [Fact]
    public void Parse_ShouldCreateCorrectCoordinate()
    {
        var coordinate = GridCoordinate.Parse("5:10");

        coordinate.Row.Should().Be(5);
        coordinate.Col.Should().Be(10);
    }

    [Fact]
    public void Parse_ShouldHandleZeroValues()
    {
        var coordinate = GridCoordinate.Parse("0:0");

        coordinate.Row.Should().Be(0);
        coordinate.Col.Should().Be(0);
    }

    [Fact]
    public void Parse_ShouldHandleLargeNumbers()
    {
        var coordinate = GridCoordinate.Parse("100:200");

        coordinate.Row.Should().Be(100);
        coordinate.Col.Should().Be(200);
    }

    [Theory]
    [InlineData("1:2", "1:2", true)]
    [InlineData("1:2", "2:1", false)]
    [InlineData("0:0", "0:0", true)]
    [InlineData("5:5", "5:6", false)]
    public void Equality_ShouldWorkCorrectly(string coord1, string coord2, bool shouldBeEqual)
    {
        var c1 = GridCoordinate.Parse(coord1);
        var c2 = GridCoordinate.Parse(coord2);

        (c1 == c2).Should().Be(shouldBeEqual);
        c1.Equals(c2).Should().Be(shouldBeEqual);
    }

    [Fact]
    public void GetHashCode_ShouldBeSameForEqualCoordinates()
    {
        var c1 = new GridCoordinate(5, 10);
        var c2 = new GridCoordinate(5, 10);

        c1.GetHashCode().Should().Be(c2.GetHashCode());
    }

    [Fact]
    public void RoundTrip_ParseAndToString_ShouldPreserveValues()
    {
        var original = new GridCoordinate(42, 17);
        var parsed = GridCoordinate.Parse(original.ToString());

        parsed.Should().Be(original);
    }
}
