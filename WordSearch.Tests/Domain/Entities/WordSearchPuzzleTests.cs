using FluentAssertions;
using WordSearch.Domain.Entities;

namespace WordSearch.Tests.Domain.Entities;

public class WordSearchPuzzleTests
{
    [Fact]
    public void Parse_ShouldCreatePuzzle_WithValidInput()
    {
        var content = @"3x3
ABC
DEF
GHI
WORD1
WORD2";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(3);
        puzzle.Dimensions.Columns.Should().Be(3);
        puzzle.Letters.GetLength(0).Should().Be(3);
        puzzle.Letters.GetLength(1).Should().Be(3);
        puzzle.Words.Should().HaveCount(2);
    }

    [Fact]
    public void Parse_ShouldExtractLettersCorrectly()
    {
        var content = @"2x3
ABC
DEF
WORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Letters[0, 0].Should().Be('A');
        puzzle.Letters[0, 1].Should().Be('B');
        puzzle.Letters[0, 2].Should().Be('C');
        puzzle.Letters[1, 0].Should().Be('D');
        puzzle.Letters[1, 1].Should().Be('E');
        puzzle.Letters[1, 2].Should().Be('F');
    }

    [Fact]
    public void Parse_ShouldExtractWordsCorrectly()
    {
        var content = @"2x2
AB
CD
WORD1
WORD2
WORD3";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Words.Should().Contain("WORD1");
        puzzle.Words.Should().Contain("WORD2");
        puzzle.Words.Should().Contain("WORD3");
    }

    [Fact]
    public void Parse_ShouldConvertLettersToUppercase()
    {
        var content = @"2x2
ab
cd
word";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Letters[0, 0].Should().Be('A');
        puzzle.Letters[1, 1].Should().Be('D');
    }

    [Fact]
    public void Parse_ShouldConvertWordsToUppercase()
    {
        var content = @"2x2
AB
CD
hello
World";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Words.Should().Contain("HELLO");
        puzzle.Words.Should().Contain("WORLD");
    }

    [Fact]
    public void Parse_ShouldHandleSpaceSeparatedLetters()
    {
        var content = @"2x3
A B C
D E F
WORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Letters[0, 0].Should().Be('A');
        puzzle.Letters[0, 1].Should().Be('B');
        puzzle.Letters[0, 2].Should().Be('C');
        puzzle.Letters[1, 0].Should().Be('D');
        puzzle.Letters[1, 1].Should().Be('E');
        puzzle.Letters[1, 2].Should().Be('F');
    }

    [Fact]
    public void Parse_ShouldHandleWindowsLineEndings()
    {
        var content = "2x2\r\nAB\r\nCD\r\nWORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(2);
        puzzle.Words.Should().Contain("WORD");
    }

    [Fact]
    public void Parse_ShouldHandleCarriageReturnOnly()
    {
        var content = "2x2\rAB\rCD\rWORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(2);
        puzzle.Words.Should().Contain("WORD");
    }

    [Fact]
    public void Parse_ShouldIgnoreEmptyLines()
    {
        var content = @"2x2
AB

CD

WORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(2);
        puzzle.Words.Should().Contain("WORD");
    }

    [Fact]
    public void Parse_ShouldTrimWhitespace()
    {
        var content = @"  2x2
  AB
  CD
  WORD  ";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Letters[0, 0].Should().Be('A');
        puzzle.Words.Should().Contain("WORD");
    }

    [Fact]
    public void Parse_ShouldThrowException_WhenInputTooShort()
    {
        var content = "2x2";

        var action = () => WordSearchPuzzle.Parse(content);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*at least dimensions and one grid row*");
    }

    [Fact]
    public void Parse_ShouldThrowException_WhenInvalidDimensions()
    {
        var content = @"invalid
AB
CD
WORD";

        var action = () => WordSearchPuzzle.Parse(content);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid dimensions format*");
    }

    [Fact]
    public void Parse_ShouldThrowException_WhenNotEnoughRows()
    {
        var content = @"5x2
AB
CD
WORD";

        var action = () => WordSearchPuzzle.Parse(content);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Expected 5 grid rows but found fewer*");
    }

    [Fact]
    public void Parse_ShouldThrowException_WhenInconsistentColumnCount()
    {
        var content = @"2x3
ABC
DE
WORD";

        var action = () => WordSearchPuzzle.Parse(content);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Row 2 has 2 letters but expected 3*");
    }

    [Fact]
    public void Parse_ShouldHandleNoWords()
    {
        var content = @"2x2
AB
CD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Words.Should().BeEmpty();
    }

    [Fact]
    public void Parse_ShouldHandleLargeGrid()
    {
        var rows = 10;
        var cols = 10;
        var gridRow = new string('A', cols);
        var content = $"{rows}x{cols}\n" + string.Join("\n", Enumerable.Repeat(gridRow, rows)) + "\nWORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(rows);
        puzzle.Dimensions.Columns.Should().Be(cols);
        puzzle.Letters.GetLength(0).Should().Be(rows);
        puzzle.Letters.GetLength(1).Should().Be(cols);
    }

    [Fact]
    public void Parse_ShouldHandleRectangularGrid_MoreColumnsThanRows()
    {
        var content = @"2x5
ABCDE
FGHIJ
WORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(2);
        puzzle.Dimensions.Columns.Should().Be(5);
    }

    [Fact]
    public void Parse_ShouldHandleRectangularGrid_MoreRowsThanColumns()
    {
        var content = @"5x2
AB
CD
EF
GH
IJ
WORD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Dimensions.Rows.Should().Be(5);
        puzzle.Dimensions.Columns.Should().Be(2);
    }

    [Fact]
    public void Parse_ShouldHandleMixedCaseWords()
    {
        var content = @"2x2
AB
CD
HeLLo
WoRLD";

        var puzzle = WordSearchPuzzle.Parse(content);

        puzzle.Words.Should().BeEquivalentTo(new[] { "HELLO", "WORLD" });
    }
}
