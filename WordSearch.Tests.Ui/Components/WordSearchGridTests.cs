using Bunit;
using FluentAssertions;
using WordSearch.Domain.ValueObjects;
using WordSearch.Web.Components;
using Xunit;
using static WordSearch.Web.Pages.Index;

namespace WordSearch.Tests.Ui.Components;

public class WordSearchGridTests : TestContext
{
    [Fact]
    public void WordSearchGrid_ShouldShowPlaceholder_WhenNoLetters()
    {
        var cut = RenderComponent<WordSearchGrid>();

        cut.Markup.Should().Contain("grid-placeholder");
        cut.Markup.Should().Contain("Upload a word search puzzle to begin");
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderGrid_WhenLettersProvided()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' },
            { 'G', 'H', 'I' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        cut.Markup.Should().NotContain("grid-placeholder");
        cut.Markup.Should().Contain("letter-grid");
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderCorrectNumberOfCells()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        var cells = cut.FindAll(".grid-cell");
        cells.Should().HaveCount(6);
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderAllLetters()
    {
        var letters = new char[,]
        {
            { 'X', 'Y', 'Z' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        cut.Markup.Should().Contain("X");
        cut.Markup.Should().Contain("Y");
        cut.Markup.Should().Contain("Z");
    }

    [Fact]
    public void WordSearchGrid_ShouldSetGridColumns_BasedOnLetterCount()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C', 'D', 'E' },
            { 'F', 'G', 'H', 'I', 'J' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        var grid = cut.Find(".letter-grid");
        grid.GetAttribute("style").Should().Contain("repeat(5, 1fr)");
    }

    [Fact]
    public void WordSearchGrid_ShouldHighlightCells_WhenHighlightedCellsProvided()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' }
        };
        var highlightedCells = new List<HighlightedCell>
        {
            new HighlightedCell(0, 0, "#ff0000"),
            new HighlightedCell(1, 2, "#00ff00")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.HighlightedCells, highlightedCells));

        var highlightedItems = cut.FindAll(".grid-cell.highlighted");
        highlightedItems.Should().HaveCount(2);
    }

    [Fact]
    public void WordSearchGrid_ShouldApplyBackgroundColor_ToHighlightedCells()
    {
        var letters = new char[,]
        {
            { 'A', 'B' }
        };
        var highlightedCells = new List<HighlightedCell>
        {
            new HighlightedCell(0, 0, "#ff5500")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.HighlightedCells, highlightedCells));

        var cell = cut.Find(".grid-cell.highlighted");
        cell.GetAttribute("style").Should().Contain("#ff5500");
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderSvgOverlay_WhenShowCirclesTrue()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "AB",
                "AB",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 1),
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        cut.Markup.Should().Contain("circle-overlay");
        cut.Markup.Should().Contain("<svg");
    }

    [Fact]
    public void WordSearchGrid_ShouldNotRenderSvgOverlay_WhenShowCirclesFalse()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "AB",
                "AB",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 1),
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, false));

        cut.Markup.Should().NotContain("circle-overlay");
    }

    [Fact]
    public void WordSearchGrid_ShouldNotRenderSvgOverlay_WhenNoCircles()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, new List<CircleData>())
            .Add(p => p.ShowCircles, true));

        cut.Markup.Should().NotContain("circle-overlay");
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderPathForEachCircle()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "AB",
                "AB",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 1),
                "#ff0000"),
            new CircleData(
                "BC",
                "BC",
                new GridCoordinate(0, 1),
                new GridCoordinate(0, 2),
                "#00ff00")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var paths = cut.FindAll("path");
        paths.Should().HaveCount(2);
    }

    [Fact]
    public void WordSearchGrid_ShouldHandleSingleRowGrid()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C', 'D', 'E' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        var cells = cut.FindAll(".grid-cell");
        cells.Should().HaveCount(5);
    }

    [Fact]
    public void WordSearchGrid_ShouldHandleSingleColumnGrid()
    {
        var letters = new char[,]
        {
            { 'A' },
            { 'B' },
            { 'C' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        var cells = cut.FindAll(".grid-cell");
        cells.Should().HaveCount(3);
    }

    [Fact]
    public void WordSearchGrid_ShouldHandleLargeGrid()
    {
        var size = 20;
        var letters = new char[size, size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                letters[i, j] = (char)('A' + ((i + j) % 26));

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        var cells = cut.FindAll(".grid-cell");
        cells.Should().HaveCount(400);
    }

    [Fact]
    public void WordSearchGrid_ShouldApplyCorrectStrokeColor_ToCircle()
    {
        var letters = new char[,]
        {
            { 'A', 'B' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "AB",
                "AB",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 1),
                "#123456")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var path = cut.Find("path");
        path.GetAttribute("stroke").Should().Be("#123456");
    }

    [Fact]
    public void WordSearchGrid_ShouldUpdateGrid_WhenLettersChange()
    {
        var initialLetters = new char[,]
        {
            { 'A', 'B' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, initialLetters));

        cut.FindAll(".grid-cell").Should().HaveCount(2);

        var newLetters = new char[,]
        {
            { 'X', 'Y', 'Z' },
            { 'P', 'Q', 'R' }
        };

        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Letters, newLetters));

        cut.FindAll(".grid-cell").Should().HaveCount(6);
    }
}
