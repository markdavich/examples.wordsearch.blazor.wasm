using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using WordSearch.Domain.ValueObjects;
using WordSearch.Web.Components;
using Xunit;
using DomainMatch = WordSearch.Domain.Entities.Match;

namespace WordSearch.Tests.Ui.Components;

public class AnswersTests : TestContext
{
    private readonly Mock<IJSRuntime> _mockJsRuntime;

    public AnswersTests()
    {
        _mockJsRuntime = new Mock<IJSRuntime>();
        Services.AddSingleton(_mockJsRuntime.Object);
    }

    [Fact]
    public void Answers_ShouldShowNoAnswersMessage_WhenEmpty()
    {
        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, new List<DomainMatch>()));

        cut.Markup.Should().Contain("No answers found");
    }

    [Fact]
    public void Answers_ShouldRenderAnswerList_WhenMatchesProvided()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2)),
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Markup.Should().NotContain("No answers found");
        cut.Markup.Should().Contain("CAT");
        cut.Markup.Should().Contain("DOG");
    }

    [Fact]
    public void Answers_ShouldRenderCorrectNumberOfListItems()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2)),
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2)),
            new DomainMatch("BIRD", new GridCoordinate(2, 0), new GridCoordinate(2, 3))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        var items = cut.FindAll(".answer-item");
        items.Should().HaveCount(3);
    }

    [Fact]
    public void Answers_ShouldDisplayCoordinates()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Markup.Should().Contain("(0:0)");
        cut.Markup.Should().Contain("(0:2)");
    }

    [Fact]
    public void Answers_ShouldHighlightAnswer_WhenInHighlightedDictionary()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };
        var highlighted = new Dictionary<string, string>
        {
            { matches[0].ToString(), "#ff0000" }
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.HighlightedAnswers, highlighted));

        var highlightedItems = cut.FindAll(".answer-item.highlighted");
        highlightedItems.Should().HaveCount(1);
    }

    [Fact]
    public void Answers_ShouldInvokeCallback_WhenAnswerClicked()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2)),
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2))
        };
        DomainMatch? clickedMatch = null;

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.OnAnswerSelected, EventCallback.Factory.Create<DomainMatch>(this, m => clickedMatch = m)));

        var firstItem = cut.FindAll(".answer-item")[0];
        firstItem.Click();

        clickedMatch.Should().NotBeNull();
        clickedMatch!.Word.Should().Be("CAT");
    }

    [Fact]
    public void Answers_ShouldRenderHeader()
    {
        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, new List<DomainMatch>()));

        cut.Find("h4").TextContent.Should().Be("Found Answers");
    }

    [Fact]
    public void Answers_ShouldRenderWordInSpan()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        var wordSpan = cut.Find(".answer-word");
        wordSpan.TextContent.Should().Be("CAT");
    }

    [Fact]
    public void Answers_ShouldRenderCoordsInSpan()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        var coordsSpan = cut.Find(".answer-coords");
        coordsSpan.TextContent.Should().Contain("(0:0)");
        coordsSpan.TextContent.Should().Contain("(0:2)");
    }

    [Fact]
    public void Answers_ShouldSetBorderColor_WhenHighlighted()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };
        var highlighted = new Dictionary<string, string>
        {
            { matches[0].ToString(), "#ff5500" }
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.HighlightedAnswers, highlighted));

        var item = cut.Find(".answer-item");
        item.GetAttribute("style").Should().Contain("#ff5500");
    }

    [Fact]
    public void Answers_ShouldHandleEmptyList()
    {
        var cut = RenderComponent<Answers>();

        cut.Markup.Should().Contain("No answers found");
    }

    [Fact]
    public void Answers_ShouldGenerateSafeIds_ForWords()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("HELLO WORLD", new GridCoordinate(0, 0), new GridCoordinate(0, 10))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Markup.Should().Contain("answer-hello-world");
    }

    [Fact]
    public void Answers_ShouldUpdateList_WhenMatchesChange()
    {
        var initialMatches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, initialMatches));

        cut.FindAll(".answer-item").Should().HaveCount(1);

        var newMatches = new List<DomainMatch>
        {
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2)),
            new DomainMatch("BIRD", new GridCoordinate(2, 0), new GridCoordinate(2, 3))
        };

        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Matches, newMatches));

        cut.FindAll(".answer-item").Should().HaveCount(2);
        cut.Markup.Should().Contain("DOG");
        cut.Markup.Should().Contain("BIRD");
    }

    [Fact]
    public void Answers_ShouldRenderAnswersContainer()
    {
        var cut = RenderComponent<Answers>();

        cut.Find(".answers-container").Should().NotBeNull();
    }

    [Fact]
    public void Answers_ShouldRenderAnswerList_WhenHasMatches()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Find(".answer-list").Should().NotBeNull();
    }

    [Fact]
    public void Answers_ShouldNotRenderAnswerList_WhenEmpty()
    {
        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, new List<DomainMatch>()));

        cut.FindAll(".answer-list").Should().BeEmpty();
    }

    [Fact]
    public void Answers_ShouldRenderNoAnswersClass()
    {
        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, new List<DomainMatch>()));

        cut.Find(".no-answers").Should().NotBeNull();
    }

    [Fact]
    public void Answers_ShouldInvokeScrollToAnswer_WhenScrollToWordProvided()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.ScrollToWord, "CAT"));

        // Verify JS was called for scrolling
        _mockJsRuntime.Verify(
            x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "eval",
                It.Is<object[]>(args => args[0].ToString()!.Contains("answer-cat"))),
            Times.Once);
    }

    [Fact]
    public void Answers_ShouldNotScrollAgain_WhenScrollToWordUnchanged()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.ScrollToWord, "CAT"));

        // Re-render with same ScrollToWord
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.ScrollToWord, "CAT"));

        // Should only have been called once
        _mockJsRuntime.Verify(
            x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "eval",
                It.IsAny<object[]>()),
            Times.Once);
    }

    [Fact]
    public void Answers_ShouldScrollAgain_WhenScrollToWordChanges()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2)),
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.ScrollToWord, "CAT"));

        // Change ScrollToWord
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.ScrollToWord, "DOG"));

        // Should have been called twice (once for each word)
        _mockJsRuntime.Verify(
            x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                "eval",
                It.IsAny<object[]>()),
            Times.Exactly(2));
    }

    [Fact]
    public void Answers_ShouldHandleMultipleHighlightedAnswers()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2)),
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2)),
            new DomainMatch("BIRD", new GridCoordinate(2, 0), new GridCoordinate(2, 3))
        };
        var highlighted = new Dictionary<string, string>
        {
            { matches[0].ToString(), "#ff0000" },
            { matches[2].ToString(), "#00ff00" }
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.HighlightedAnswers, highlighted));

        var highlightedItems = cut.FindAll(".answer-item.highlighted");
        highlightedItems.Should().HaveCount(2);
    }

    [Fact]
    public void Answers_ShouldHandleLongWordList()
    {
        var matches = new List<DomainMatch>();
        for (int i = 0; i < 50; i++)
        {
            matches.Add(new DomainMatch($"WORD{i}", new GridCoordinate(i, 0), new GridCoordinate(i, 4)));
        }

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.FindAll(".answer-item").Should().HaveCount(50);
    }

    [Fact]
    public async Task Answers_ShouldSelectSecondMatch_WhenClicked()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2)),
            new DomainMatch("DOG", new GridCoordinate(1, 0), new GridCoordinate(1, 2))
        };
        DomainMatch? selectedMatch = null;

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.OnAnswerSelected, EventCallback.Factory.Create<DomainMatch>(this, m => selectedMatch = m)));

        var secondItem = cut.FindAll(".answer-item")[1];
        await cut.InvokeAsync(() => secondItem.Click());

        selectedMatch.Should().NotBeNull();
        selectedMatch!.Word.Should().Be("DOG");
    }

    [Fact]
    public void Answers_ShouldDisplayStartCoordinates()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("TEST", new GridCoordinate(5, 10), new GridCoordinate(5, 13))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Markup.Should().Contain("(5:10)");
    }

    [Fact]
    public void Answers_ShouldDisplayEndCoordinates()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("TEST", new GridCoordinate(5, 10), new GridCoordinate(5, 13))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Markup.Should().Contain("(5:13)");
    }

    [Fact]
    public void Answers_ShouldHandleSpecialCharactersInWord()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("TEST-WORD", new GridCoordinate(0, 0), new GridCoordinate(0, 8))
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches));

        cut.Markup.Should().Contain("answer-test-word");
        cut.Markup.Should().Contain("TEST-WORD");
    }

    [Fact]
    public void Answers_ShouldClearHighlightsWhenDictionaryEmpty()
    {
        var matches = new List<DomainMatch>
        {
            new DomainMatch("CAT", new GridCoordinate(0, 0), new GridCoordinate(0, 2))
        };
        var highlighted = new Dictionary<string, string>
        {
            { matches[0].ToString(), "#ff0000" }
        };

        var cut = RenderComponent<Answers>(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.HighlightedAnswers, highlighted));

        cut.FindAll(".answer-item.highlighted").Should().HaveCount(1);

        // Clear highlights
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Matches, matches)
            .Add(p => p.HighlightedAnswers, new Dictionary<string, string>()));

        cut.FindAll(".answer-item.highlighted").Should().BeEmpty();
    }
}
