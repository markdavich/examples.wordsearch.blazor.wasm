using FluentAssertions;
using WordSearch.Application.Services;
using WordSearch.Domain.Enums;

namespace WordSearch.Tests.Application.Services;

public class MatrixServiceTests
{
    [Fact]
    public void Transpose_ShouldSwapRowsAndColumns()
    {
        var matrix = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        var result = MatrixService.Transpose(matrix);

        result.GetLength(0).Should().Be(3);
        result.GetLength(1).Should().Be(2);
        result[0, 0].Should().Be(1);
        result[0, 1].Should().Be(4);
        result[1, 0].Should().Be(2);
        result[1, 1].Should().Be(5);
        result[2, 0].Should().Be(3);
        result[2, 1].Should().Be(6);
    }

    [Fact]
    public void Transpose_ShouldHandleSquareMatrix()
    {
        var matrix = new char[,]
        {
            { 'A', 'B' },
            { 'C', 'D' }
        };

        var result = MatrixService.Transpose(matrix);

        result.GetLength(0).Should().Be(2);
        result.GetLength(1).Should().Be(2);
        result[0, 0].Should().Be('A');
        result[0, 1].Should().Be('C');
        result[1, 0].Should().Be('B');
        result[1, 1].Should().Be('D');
    }

    [Fact]
    public void Transpose_ShouldHandleSingleRowMatrix()
    {
        var matrix = new int[,] { { 1, 2, 3, 4 } };

        var result = MatrixService.Transpose(matrix);

        result.GetLength(0).Should().Be(4);
        result.GetLength(1).Should().Be(1);
        result[0, 0].Should().Be(1);
        result[3, 0].Should().Be(4);
    }

    [Fact]
    public void Transpose_ShouldHandleSingleColumnMatrix()
    {
        var matrix = new int[,]
        {
            { 1 },
            { 2 },
            { 3 }
        };

        var result = MatrixService.Transpose(matrix);

        result.GetLength(0).Should().Be(1);
        result.GetLength(1).Should().Be(3);
    }

    [Fact]
    public void Transpose_ShouldBeReversible()
    {
        var original = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' }
        };

        var transposed = MatrixService.Transpose(original);
        var restored = MatrixService.Transpose(transposed);

        restored.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void GetShape_ShouldReturnTall_WhenMoreRowsThanColumns()
    {
        var shape = MatrixService.GetShape(10, 5);

        shape.Should().Be(MatrixShape.Tall);
    }

    [Fact]
    public void GetShape_ShouldReturnWide_WhenMoreColumnsThanRows()
    {
        var shape = MatrixService.GetShape(5, 10);

        shape.Should().Be(MatrixShape.Wide);
    }

    [Fact]
    public void GetShape_ShouldReturnSquare_WhenEqualRowsAndColumns()
    {
        var shape = MatrixService.GetShape(5, 5);

        shape.Should().Be(MatrixShape.Square);
    }

    [Fact]
    public void Flatten_ShouldConvertGridToString()
    {
        var grid = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' }
        };

        var result = MatrixService.Flatten(grid);

        result.Should().Be("ABCDEF");
    }

    [Fact]
    public void Flatten_ShouldHandleSingleRow()
    {
        var grid = new char[,] { { 'H', 'E', 'L', 'L', 'O' } };

        var result = MatrixService.Flatten(grid);

        result.Should().Be("HELLO");
    }

    [Fact]
    public void Flatten_ShouldHandleSingleColumn()
    {
        var grid = new char[,]
        {
            { 'A' },
            { 'B' },
            { 'C' }
        };

        var result = MatrixService.Flatten(grid);

        result.Should().Be("ABC");
    }

    [Fact]
    public void IndexToCoordinates_ShouldReturnCorrectCoordinates()
    {
        var (row, col) = MatrixService.IndexToCoordinates(5, 3);

        row.Should().Be(1);
        col.Should().Be(2);
    }

    [Fact]
    public void IndexToCoordinates_ShouldReturnZeroForFirstElement()
    {
        var (row, col) = MatrixService.IndexToCoordinates(0, 5);

        row.Should().Be(0);
        col.Should().Be(0);
    }

    [Fact]
    public void IndexToCoordinates_ShouldReturnLastColumnOfFirstRow()
    {
        var (row, col) = MatrixService.IndexToCoordinates(4, 5);

        row.Should().Be(0);
        col.Should().Be(4);
    }

    [Fact]
    public void IndexToCoordinates_ShouldReturnFirstColumnOfSecondRow()
    {
        var (row, col) = MatrixService.IndexToCoordinates(5, 5);

        row.Should().Be(1);
        col.Should().Be(0);
    }

    [Theory]
    [InlineData(0, 4, 0, 0)]
    [InlineData(3, 4, 0, 3)]
    [InlineData(4, 4, 1, 0)]
    [InlineData(7, 4, 1, 3)]
    [InlineData(11, 4, 2, 3)]
    public void IndexToCoordinates_ShouldWorkForVariousIndices(int index, int columnCount, int expectedRow, int expectedCol)
    {
        var (row, col) = MatrixService.IndexToCoordinates(index, columnCount);

        row.Should().Be(expectedRow);
        col.Should().Be(expectedCol);
    }
}
