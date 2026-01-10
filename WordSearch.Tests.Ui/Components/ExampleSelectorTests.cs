using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using WordSearch.Web.Components;
using WordSearch.Web.Services;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class ExampleSelectorTests : TestContext
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly PuzzleService _puzzleService;

    public ExampleSelectorTests()
    {
        _puzzleService = new PuzzleService();
        Services.AddSingleton(_puzzleService);

        _mockHttp = new MockHttpMessageHandler();
        _mockHttp.Fallback.Respond("text/plain", "10x10\nABCDEFGHIJ\nKLMNOPQRST\nTEST");
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost/");
        Services.AddSingleton(httpClient);
    }

    [Fact]
    public void ExampleSelector_ShouldRenderSelect()
    {
        var cut = RenderComponent<ExampleSelector>();

        var select = cut.Find("select.example-select");
        select.Should().NotBeNull();
    }

    [Fact]
    public void ExampleSelector_ShouldRenderDefaultOption()
    {
        var cut = RenderComponent<ExampleSelector>();

        cut.Markup.Should().Contain("Examples...");
    }

    [Fact]
    public void ExampleSelector_ShouldRenderAllExampleOptions()
    {
        var cut = RenderComponent<ExampleSelector>();

        cut.Markup.Should().Contain("Ai");
        cut.Markup.Should().Contain("GitHub");
        cut.Markup.Should().Contain("Enlighten");
        cut.Markup.Should().Contain("ABC");
        cut.Markup.Should().Contain("Numbers");
        cut.Markup.Should().Contain("Hello");
    }

    [Fact]
    public void ExampleSelector_ShouldHaveCorrectNumberOfOptions()
    {
        var cut = RenderComponent<ExampleSelector>();

        var options = cut.FindAll("select.example-select option");
        // 1 placeholder + 6 examples = 7
        options.Should().HaveCount(7);
    }

    [Fact]
    public void ExampleSelector_ShouldRenderCorrectExamplePaths()
    {
        var cut = RenderComponent<ExampleSelector>();

        cut.Markup.Should().Contain("assets/Ai.txt");
        cut.Markup.Should().Contain("assets/GitHub.txt");
        cut.Markup.Should().Contain("assets/Enlighten.txt");
        cut.Markup.Should().Contain("assets/ABC.txt");
        cut.Markup.Should().Contain("assets/Numbers.txt");
        cut.Markup.Should().Contain("assets/Hello.txt");
    }

    [Fact]
    public void ExampleSelector_ShouldHaveTooltipWrapper()
    {
        var cut = RenderComponent<ExampleSelector>();

        cut.Find(".tooltip-wrapper").Should().NotBeNull();
    }

    [Fact]
    public void ExampleSelector_ShouldHaveEmptyDefaultValue()
    {
        var cut = RenderComponent<ExampleSelector>();

        var options = cut.FindAll("select.example-select option");
        options[0].GetAttribute("value").Should().BeEmpty();
    }

    [Fact]
    public async Task ExampleSelector_ShouldLoadExample_WhenSelected()
    {
        string? loadedContent = null;
        _puzzleService.OnPuzzleLoaded += content => loadedContent = content;

        var cut = RenderComponent<ExampleSelector>();

        var select = cut.Find("select.example-select");
        await cut.InvokeAsync(() => select.Change("assets/Ai.txt"));

        // Wait for async operation
        cut.WaitForState(() => loadedContent != null, TimeSpan.FromSeconds(1));

        loadedContent.Should().NotBeNull();
    }

    [Fact]
    public void ExampleSelector_ShouldNotLoadExample_WhenEmptyValueSelected()
    {
        string? loadedContent = null;
        _puzzleService.OnPuzzleLoaded += content => loadedContent = content;

        var cut = RenderComponent<ExampleSelector>();

        var select = cut.Find("select.example-select");
        select.Change("");

        loadedContent.Should().BeNull();
    }
}
