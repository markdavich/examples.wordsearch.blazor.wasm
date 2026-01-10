using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using WordSearch.Web.Components;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class FileInputTests : TestContext
{
    public FileInputTests()
    {
        // Register a mock HttpClient for components that inject it
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Fallback.Respond("text/plain", "test content");
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost/");
        Services.AddSingleton(httpClient);
    }

    [Fact]
    public void FileInput_ShouldRenderLabel()
    {
        var cut = RenderComponent<FileInput>(parameters => parameters
            .Add(p => p.Label, "Upload Puzzle"));

        cut.Markup.Should().Contain("Upload Puzzle");
    }

    [Fact]
    public void FileInput_ShouldRenderDefaultLabel()
    {
        var cut = RenderComponent<FileInput>();

        cut.Markup.Should().Contain("Select File");
    }

    [Fact]
    public void FileInput_ShouldRenderFileInput()
    {
        var cut = RenderComponent<FileInput>();

        var input = cut.Find("input[type='file']");
        input.Should().NotBeNull();
    }

    [Fact]
    public void FileInput_ShouldAcceptCorrectFileTypes()
    {
        var cut = RenderComponent<FileInput>();

        var input = cut.Find("input[type='file']");
        input.GetAttribute("accept").Should().Contain(".wordsearch");
        input.GetAttribute("accept").Should().Contain(".txt");
    }

    [Fact]
    public void FileInput_ShouldRenderExampleDropdown()
    {
        var cut = RenderComponent<FileInput>();

        var select = cut.Find("select.example-select");
        select.Should().NotBeNull();
    }

    [Fact]
    public void FileInput_ShouldRenderExampleOptions()
    {
        var cut = RenderComponent<FileInput>();

        cut.Markup.Should().Contain("Load Example...");
        cut.Markup.Should().Contain("Ai");
        cut.Markup.Should().Contain("GitHub");
        cut.Markup.Should().Contain("Enlighten");
        cut.Markup.Should().Contain("ABC");
        cut.Markup.Should().Contain("Numbers");
        cut.Markup.Should().Contain("Hello");
    }

    [Fact]
    public void FileInput_ShouldRenderOrSeparator()
    {
        var cut = RenderComponent<FileInput>();

        cut.Markup.Should().Contain("or");
        cut.Find(".or-separator").Should().NotBeNull();
    }

    [Fact]
    public void FileInput_ShouldNotShowFileName_Initially()
    {
        var cut = RenderComponent<FileInput>();

        cut.Markup.Should().NotContain("file-name");
    }

    [Fact]
    public void FileInput_ShouldNotShowError_Initially()
    {
        var cut = RenderComponent<FileInput>();

        cut.Markup.Should().NotContain("file-error");
    }

    [Fact]
    public void FileInput_ShouldHaveContainer()
    {
        var cut = RenderComponent<FileInput>();

        cut.Find(".file-input-container").Should().NotBeNull();
    }

    [Fact]
    public void FileInput_ShouldHaveInputRow()
    {
        var cut = RenderComponent<FileInput>();

        cut.Find(".file-input-row").Should().NotBeNull();
    }

    [Fact]
    public void FileInput_ShouldHaveTooltips()
    {
        var cut = RenderComponent<FileInput>();

        // The component uses Tooltip wrapper around inputs
        cut.FindAll(".tooltip-wrapper").Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public void FileInput_ShouldRenderCorrectExamplePaths()
    {
        var cut = RenderComponent<FileInput>();

        cut.Markup.Should().Contain("assets/Ai.txt");
        cut.Markup.Should().Contain("assets/GitHub.txt");
        cut.Markup.Should().Contain("assets/Enlighten.txt");
        cut.Markup.Should().Contain("assets/ABC.txt");
        cut.Markup.Should().Contain("assets/Numbers.txt");
        cut.Markup.Should().Contain("assets/Hello.txt");
    }

    [Fact]
    public void FileInput_ShouldRenderLabelElement()
    {
        var cut = RenderComponent<FileInput>(parameters => parameters
            .Add(p => p.Label, "Test Label"));

        var label = cut.Find(".file-input-label");
        label.Should().NotBeNull();
        label.TextContent.Should().Be("Test Label");
    }

    [Fact]
    public void FileInput_ShouldHaveCorrectDefaultPlaceholderOption()
    {
        var cut = RenderComponent<FileInput>();

        var options = cut.FindAll("select.example-select option");
        options.Should().HaveCountGreaterThan(1);
        options[0].TextContent.Should().Be("Load Example...");
        options[0].GetAttribute("value").Should().BeEmpty();
    }

    [Fact]
    public void FileInput_ShouldHaveCorrectNumberOfExamples()
    {
        var cut = RenderComponent<FileInput>();

        var options = cut.FindAll("select.example-select option");
        // 1 placeholder + 6 examples = 7
        options.Should().HaveCount(7);
    }

    [Fact]
    public void FileInput_ShouldRenderFileInputClass()
    {
        var cut = RenderComponent<FileInput>();

        var input = cut.Find("input[type='file']");
        input.GetAttribute("class").Should().Contain("file-input");
    }
}
