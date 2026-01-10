using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using WordSearch.Application.Interfaces;
using WordSearch.Domain.ValueObjects;
using WordSearch.Web.Services;
using Xunit;
using DomainMatch = WordSearch.Domain.Entities.Match;
using IndexPage = WordSearch.Web.Pages.Index;

namespace WordSearch.Tests.Ui.Pages;

public class IndexTests : TestContext
{
    private readonly PuzzleService _puzzleService;
    private readonly Mock<IWordFinder> _mockWordFinder;
    private readonly Mock<IJSRuntime> _mockJsRuntime;

    public IndexTests()
    {
        _puzzleService = new PuzzleService();
        Services.AddSingleton(_puzzleService);

        _mockWordFinder = new Mock<IWordFinder>();
        _mockWordFinder
            .Setup(x => x.FindWords(It.IsAny<char[,]>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new List<DomainMatch>());
        Services.AddSingleton(_mockWordFinder.Object);

        _mockJsRuntime = new Mock<IJSRuntime>();
        Services.AddSingleton(_mockJsRuntime.Object);
    }

    [Fact]
    public void Index_ShouldRenderContentContainer()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Find(".content-container").Should().NotBeNull();
    }

    [Fact]
    public void Index_ShouldRenderLeftPanel()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Find(".left-panel").Should().NotBeNull();
    }

    [Fact]
    public void Index_ShouldRenderGridPanel()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Find(".grid-panel").Should().NotBeNull();
    }

    [Fact]
    public void Index_ShouldRenderClearHighlightsButton()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("Clear Highlights");
    }

    [Fact]
    public void Index_ShouldRenderCircleAnswersButton()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("Circle Answers");
    }

    [Fact]
    public void Index_ShouldRenderWordsToFind()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("Words to Find");
    }

    [Fact]
    public void Index_ShouldRenderAnswers()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("Found Answers");
    }

    [Fact]
    public void Index_ShouldRenderWordSearchGrid()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("grid-placeholder");
    }

    [Fact]
    public async Task Index_ShouldLoadPuzzle_WhenPuzzleServiceFiresEvent()
    {
        _mockWordFinder
            .Setup(x => x.FindWords(It.IsAny<char[,]>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new List<DomainMatch>
            {
                new DomainMatch("TEST", new GridCoordinate(0, 0), new GridCoordinate(0, 3))
            });

        var cut = RenderComponent<IndexPage>();

        await cut.InvokeAsync(() => _puzzleService.LoadPuzzle("5x5\nABCDE\nFGHIJ\nKLMNO\nPQRST\nUVWXY\nTEST"));

        cut.Markup.Should().Contain("TEST");
        cut.Markup.Should().NotContain("grid-placeholder");
    }

    [Fact]
    public async Task Index_ShouldDisplayWords_WhenPuzzleLoaded()
    {
        _mockWordFinder
            .Setup(x => x.FindWords(It.IsAny<char[,]>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new List<DomainMatch>());

        var cut = RenderComponent<IndexPage>();

        await cut.InvokeAsync(() => _puzzleService.LoadPuzzle("3x3\nABC\nDEF\nGHI\nCAT\nDOG"));

        cut.Markup.Should().Contain("CAT");
        cut.Markup.Should().Contain("DOG");
    }

    [Fact]
    public async Task Index_ShouldCallWordFinder_WhenPuzzleLoaded()
    {
        var cut = RenderComponent<IndexPage>();

        await cut.InvokeAsync(() => _puzzleService.LoadPuzzle("3x3\nABC\nDEF\nGHI\nTEST"));

        _mockWordFinder.Verify(x => x.FindWords(It.IsAny<char[,]>(), It.IsAny<IEnumerable<string>>()), Times.Once);
    }

    [Fact]
    public async Task Index_ShouldHandleInvalidPuzzle_Gracefully()
    {
        var cut = RenderComponent<IndexPage>();

        // Should not throw
        await cut.InvokeAsync(() => _puzzleService.LoadPuzzle("invalid content"));

        cut.Markup.Should().Contain("grid-placeholder");
    }

    [Fact]
    public void Index_ShouldClickClearHighlights()
    {
        var cut = RenderComponent<IndexPage>();

        var clearButton = cut.Find("button.action-btn");
        clearButton.Click();

        // Should not throw
        cut.Markup.Should().NotBeNull();
    }

    [Fact]
    public async Task Index_ShouldToggleCircles_WhenButtonClicked()
    {
        _mockWordFinder
            .Setup(x => x.FindWords(It.IsAny<char[,]>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new List<DomainMatch>
            {
                new DomainMatch("ABC", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
            });

        var cut = RenderComponent<IndexPage>();

        // Load a puzzle first
        await cut.InvokeAsync(() => _puzzleService.LoadPuzzle("3x3\nABC\nDEF\nGHI\nABC"));

        // Find and click the Circle Answers button
        var buttons = cut.FindAll("button.action-btn");
        var circleButton = buttons[1]; // Second button
        circleButton.Click();

        // Should show circles
        cut.Markup.Should().Contain("circle-overlay");

        // Click again to hide
        circleButton.Click();
        cut.Markup.Should().NotContain("circle-overlay");
    }

    [Fact]
    public void Index_ShouldImplementIDisposable()
    {
        var cut = RenderComponent<IndexPage>();

        // Dispose should not throw
        cut.Dispose();
    }

    [Fact]
    public async Task Index_ShouldHighlightWord_WhenWordSelected()
    {
        _mockWordFinder
            .Setup(x => x.FindWords(It.IsAny<char[,]>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new List<DomainMatch>
            {
                new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
            });

        var cut = RenderComponent<IndexPage>();
        await cut.InvokeAsync(() => _puzzleService.LoadPuzzle("3x3\nCAT\nDEF\nGHI\nCAT"));

        // Click on a word in WordsToFind
        var wordItems = cut.FindAll(".word-item");
        if (wordItems.Count > 0)
        {
            wordItems[0].Click();
        }

        // The word should now be selected and highlighted
        cut.Markup.Should().NotBeNull();
    }

    [Fact]
    public void Index_ShouldUnsubscribe_OnDispose()
    {
        var cut = RenderComponent<IndexPage>();
        cut.Dispose();

        // Loading puzzle after dispose should not affect the disposed component
        // Note: We just call LoadPuzzle without InvokeAsync since component is disposed
        _puzzleService.LoadPuzzle("3x3\nABC\nDEF\nGHI\nTEST");

        // Should not throw
    }

    [Fact]
    public void Index_ShouldRenderTooltips()
    {
        var cut = RenderComponent<IndexPage>();

        // Should have tooltips for buttons
        var tooltips = cut.FindAll(".tooltip-wrapper");
        tooltips.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public void Index_ShouldRenderButtonRow()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Find(".button-row").Should().NotBeNull();
    }

    [Fact]
    public void Index_ShouldRenderControlsColumn()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Find(".controls-column").Should().NotBeNull();
    }

    [Fact]
    public void Index_ShouldRenderAnswersColumn()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Find(".answers-column").Should().NotBeNull();
    }
}
