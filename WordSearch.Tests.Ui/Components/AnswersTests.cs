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
    public AnswersTests()
    {
        var mockJsRuntime = new Mock<IJSRuntime>();
        Services.AddSingleton(mockJsRuntime.Object);
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
}
