using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using WordSearch.Domain.ValueObjects;
using WordSearch.Web.Components;
using Xunit;
using static WordSearch.Web.Pages.Index;

namespace WordSearch.Tests.Ui.Components;

public class WordSearchGridTests : TestContext
{
    private readonly Mock<IJSRuntime> _mockJsRuntime;

    public WordSearchGridTests()
    {
        _mockJsRuntime = new Mock<IJSRuntime>();
        Services.AddSingleton(_mockJsRuntime.Object);
    }

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

    [Fact]
    public async Task WordSearchGrid_ShouldFireOnCircleHover_WhenCircleEntered()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "ABC",
                "ABC",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 2),
                "#ff0000")
        };
        string? hoveredAnswer = null;

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true)
            .Add(p => p.OnCircleHover, EventCallback.Factory.Create<string?>(this, a => hoveredAnswer = a)));

        var path = cut.Find("path");
        await cut.InvokeAsync(() => path.TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs()));

        hoveredAnswer.Should().Be("ABC");
    }

    [Fact]
    public async Task WordSearchGrid_ShouldFireOnCircleHover_WhenCircleLeft()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "ABC",
                "ABC",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 2),
                "#ff0000")
        };
        string? hoveredAnswer = "initial";

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true)
            .Add(p => p.OnCircleHover, EventCallback.Factory.Create<string?>(this, a => hoveredAnswer = a)));

        var path = cut.Find("path");
        await cut.InvokeAsync(() => path.TriggerEventAsync("onmouseleave", new Microsoft.AspNetCore.Components.Web.MouseEventArgs()));

        hoveredAnswer.Should().BeNull();
    }

    [Fact]
    public async Task WordSearchGrid_ShouldHighlightPath_WhenCircleHovered()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "ABC",
                "ABC",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 2),
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var path = cut.Find("path");
        await cut.InvokeAsync(() => path.TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs()));

        // Path should have increased stroke width when hovered
        cut.Find("path").GetAttribute("stroke-width").Should().Be("5");
        cut.Find("path").GetAttribute("class").Should().Contain("hovered");
    }

    [Fact]
    public async Task WordSearchGrid_ShouldElevateCells_WhenCircleHovered()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "ABC",
                "ABC",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 2),
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var path = cut.Find("path");
        await cut.InvokeAsync(() => path.TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs()));

        // All cells in the answer should be elevated
        var elevatedCells = cut.FindAll(".grid-cell.elevated");
        elevatedCells.Should().HaveCount(3); // A, B, C all elevated
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderGridContainer()
    {
        var letters = new char[,]
        {
            { 'A', 'B' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        cut.Find(".grid-container").Should().NotBeNull();
    }

    [Fact]
    public void WordSearchGrid_ShouldRenderGridWrapper()
    {
        var letters = new char[,]
        {
            { 'A', 'B' }
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters));

        cut.Find(".grid-wrapper").Should().NotBeNull();
    }

    [Fact]
    public void WordSearchGrid_ShouldHandleDiagonalCircle()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C' },
            { 'D', 'E', 'F' },
            { 'G', 'H', 'I' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "AEI",
                "AEI",
                new GridCoordinate(0, 0),
                new GridCoordinate(2, 2),
                "#00ff00")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        // Should render the diagonal path
        cut.Find("path").Should().NotBeNull();
        cut.Find("path").GetAttribute("d").Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void WordSearchGrid_ShouldHandleVerticalCircle()
    {
        var letters = new char[,]
        {
            { 'A' },
            { 'B' },
            { 'C' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "ABC",
                "ABC",
                new GridCoordinate(0, 0),
                new GridCoordinate(2, 0),
                "#0000ff")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        cut.Find("path").Should().NotBeNull();
    }

    [Fact]
    public void WordSearchGrid_ShouldApplyFillOpacity_WhenNotHovered()
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
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var path = cut.Find("path");
        path.GetAttribute("fill-opacity").Should().Be("0");
    }

    [Fact]
    public async Task WordSearchGrid_ShouldApplyFillOpacity_WhenHovered()
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
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var path = cut.Find("path");
        await cut.InvokeAsync(() => path.TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs()));

        path = cut.Find("path");
        path.GetAttribute("fill-opacity").Should().Be("0.3");
    }

    [Fact]
    public void WordSearchGrid_ShouldSetSvgDimensions_BasedOnGrid()
    {
        var letters = new char[,]
        {
            { 'A', 'B', 'C', 'D', 'E' },
            { 'F', 'G', 'H', 'I', 'J' },
            { 'K', 'L', 'M', 'N', 'O' }
        };
        var circles = new List<CircleData>
        {
            new CircleData(
                "ABC",
                "ABC",
                new GridCoordinate(0, 0),
                new GridCoordinate(0, 2),
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var svg = cut.Find("svg.circle-overlay");
        svg.GetAttribute("width").Should().Be("200"); // 5 cols * 40
        svg.GetAttribute("height").Should().Be("120"); // 3 rows * 40
    }

    [Fact]
    public void WordSearchGrid_ShouldHaveAnswerCircleClass_OnPath()
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
                "#ff0000")
        };

        var cut = RenderComponent<WordSearchGrid>(parameters => parameters
            .Add(p => p.Letters, letters)
            .Add(p => p.Circles, circles)
            .Add(p => p.ShowCircles, true));

        var path = cut.Find("path");
        path.GetAttribute("class").Should().Contain("answer-circle");
    }
}
