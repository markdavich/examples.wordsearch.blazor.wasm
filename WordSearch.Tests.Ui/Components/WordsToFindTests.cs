using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using WordSearch.Web.Components;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class WordsToFindTests : TestContext
{
    [Fact]
    public void WordsToFind_ShouldShowNoWordsMessage_WhenEmpty()
    {
        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, new List<string>()));

        cut.Markup.Should().Contain("No puzzle loaded");
    }

    [Fact]
    public void WordsToFind_ShouldRenderWordList_WhenWordsProvided()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words));

        cut.Markup.Should().NotContain("No puzzle loaded");
        cut.Markup.Should().Contain("CAT");
        cut.Markup.Should().Contain("DOG");
        cut.Markup.Should().Contain("BIRD");
    }

    [Fact]
    public void WordsToFind_ShouldRenderCorrectNumberOfListItems()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD", "FISH" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words));

        var items = cut.FindAll(".word-item");
        items.Should().HaveCount(4);
    }

    [Fact]
    public void WordsToFind_ShouldHighlightSelectedWord()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words)
            .Add(p => p.SelectedWord, "DOG"));

        var selectedItems = cut.FindAll(".word-item.selected");
        selectedItems.Should().HaveCount(1);
        selectedItems[0].TextContent.Should().Be("DOG");
    }

    [Fact]
    public void WordsToFind_ShouldInvokeCallback_WhenWordClicked()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD" };
        string? clickedWord = null;

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words)
            .Add(p => p.OnWordSelected, EventCallback.Factory.Create<string>(this, w => clickedWord = w)));

        var catItem = cut.FindAll(".word-item")[0];
        catItem.Click();

        clickedWord.Should().Be("CAT");
    }

    [Fact]
    public void WordsToFind_ShouldBeCaseInsensitive_ForSelection()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words)
            .Add(p => p.SelectedWord, "cat"));

        var selectedItems = cut.FindAll(".word-item.selected");
        selectedItems.Should().HaveCount(1);
        selectedItems[0].TextContent.Should().Be("CAT");
    }

    [Fact]
    public void WordsToFind_ShouldNotHaveSelectedItems_WhenNoSelection()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words));

        var selectedItems = cut.FindAll(".word-item.selected");
        selectedItems.Should().BeEmpty();
    }

    [Fact]
    public void WordsToFind_ShouldRenderHeader()
    {
        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, new List<string>()));

        cut.Find("h4").TextContent.Should().Be("Words to Find");
    }

    [Fact]
    public void WordsToFind_ShouldRenderUnorderedList_WhenWordsExist()
    {
        var words = new List<string> { "CAT" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words));

        cut.Find(".word-list").Should().NotBeNull();
        cut.Find(".word-list").TagName.Should().Be("UL");
    }

    [Fact]
    public void WordsToFind_ShouldHandleEmptyList()
    {
        var cut = RenderComponent<WordsToFind>();

        cut.Markup.Should().Contain("No puzzle loaded");
    }

    [Fact]
    public void WordsToFind_ShouldUpdateSelection_WhenParameterChanges()
    {
        var words = new List<string> { "CAT", "DOG", "BIRD" };

        var cut = RenderComponent<WordsToFind>(parameters => parameters
            .Add(p => p.Words, words)
            .Add(p => p.SelectedWord, "CAT"));

        cut.FindAll(".word-item.selected")[0].TextContent.Should().Be("CAT");

        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Words, words)
            .Add(p => p.SelectedWord, "BIRD"));

        cut.FindAll(".word-item.selected")[0].TextContent.Should().Be("BIRD");
    }
}
