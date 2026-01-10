using FluentAssertions;
using WordSearch.Domain.ValueObjects;

namespace WordSearch.Tests.Domain.ValueObjects;

public class DimensionsTests
{
    [Fact]
    public void Constructor_ShouldSetRowsAndColumns()
    {
        var dimensions = new Dimensions(10, 15);

        dimensions.Rows.Should().Be(10);
        dimensions.Columns.Should().Be(15);
    }

    [Fact]
    public void Constructor_ShouldHandleSquareDimensions()
    {
        var dimensions = new Dimensions(5, 5);

        dimensions.Rows.Should().Be(5);
        dimensions.Columns.Should().Be(5);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        var dimensions = new Dimensions(10, 15);

        dimensions.ToString().Should().Be("10x15");
    }

    [Fact]
    public void ToString_ShouldHandleSquareDimensions()
    {
        var dimensions = new Dimensions(8, 8);

        dimensions.ToString().Should().Be("8x8");
    }

    [Fact]
    public void Parse_ShouldCreateCorrectDimensions()
    {
        var dimensions = Dimensions.Parse("10x15");

        dimensions.Rows.Should().Be(10);
        dimensions.Columns.Should().Be(15);
    }

    [Fact]
    public void Parse_ShouldBeCaseInsensitive()
    {
        var dimensions = Dimensions.Parse("10X15");

        dimensions.Rows.Should().Be(10);
        dimensions.Columns.Should().Be(15);
    }

    [Fact]
    public void Parse_ShouldHandleSquareDimensions()
    {
        var dimensions = Dimensions.Parse("5x5");

        dimensions.Rows.Should().Be(5);
        dimensions.Columns.Should().Be(5);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidInput()
    {
        var result = Dimensions.TryParse("10x15", out var dimensions);

        result.Should().BeTrue();
        dimensions.Rows.Should().Be(10);
        dimensions.Columns.Should().Be(15);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidFormat()
    {
        var result = Dimensions.TryParse("10-15", out var dimensions);

        result.Should().BeFalse();
        dimensions.Should().Be(default(Dimensions));
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForNonNumericValues()
    {
        var result = Dimensions.TryParse("abcxdef", out var dimensions);

        result.Should().BeFalse();
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForSingleValue()
    {
        var result = Dimensions.TryParse("10", out var dimensions);

        result.Should().BeFalse();
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForEmptyString()
    {
        var result = Dimensions.TryParse("", out var dimensions);

        result.Should().BeFalse();
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForTooManyParts()
    {
        var result = Dimensions.TryParse("10x15x20", out var dimensions);

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("10x15", "10x15", true)]
    [InlineData("10x15", "15x10", false)]
    [InlineData("5x5", "5x5", true)]
    public void Equality_ShouldWorkCorrectly(string dim1, string dim2, bool shouldBeEqual)
    {
        var d1 = Dimensions.Parse(dim1);
        var d2 = Dimensions.Parse(dim2);

        (d1 == d2).Should().Be(shouldBeEqual);
        d1.Equals(d2).Should().Be(shouldBeEqual);
    }

    [Fact]
    public void GetHashCode_ShouldBeSameForEqualDimensions()
    {
        var d1 = new Dimensions(10, 15);
        var d2 = new Dimensions(10, 15);

        d1.GetHashCode().Should().Be(d2.GetHashCode());
    }

    [Fact]
    public void RoundTrip_ParseAndToString_ShouldPreserveValues()
    {
        var original = new Dimensions(42, 17);
        var parsed = Dimensions.Parse(original.ToString());

        parsed.Should().Be(original);
    }
}
