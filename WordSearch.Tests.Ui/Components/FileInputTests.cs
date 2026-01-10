using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using WordSearch.Web.Components;
using WordSearch.Web.Services;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class FileInputTests : TestContext
{
    public FileInputTests()
    {
        // Register PuzzleService for components that need it
        Services.AddSingleton<PuzzleService>();
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
    public void FileInput_ShouldNotShowFileName_Initially()
    {
        var cut = RenderComponent<FileInput>();

        cut.FindAll(".file-name").Should().BeEmpty();
    }

    [Fact]
    public void FileInput_ShouldNotShowError_Initially()
    {
        var cut = RenderComponent<FileInput>();

        cut.FindAll(".file-error").Should().BeEmpty();
    }

    [Fact]
    public void FileInput_ShouldHaveContainer()
    {
        var cut = RenderComponent<FileInput>();

        cut.Find(".file-input-container").Should().NotBeNull();
    }

    [Fact]
    public void FileInput_ShouldHaveTooltip()
    {
        var cut = RenderComponent<FileInput>();

        // The component uses Tooltip wrapper around the label
        cut.FindAll(".tooltip-wrapper").Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public void FileInput_ShouldRenderFileInputClass()
    {
        var cut = RenderComponent<FileInput>();

        var input = cut.Find("input[type='file']");
        input.GetAttribute("class").Should().Contain("file-input");
    }

    [Fact]
    public void FileInput_ShouldRenderH4Label()
    {
        var cut = RenderComponent<FileInput>(parameters => parameters
            .Add(p => p.Label, "Test Label"));

        var h4 = cut.Find("h4");
        h4.Should().NotBeNull();
        h4.TextContent.Should().Be("Test Label");
    }

    [Fact]
    public void FileInput_ShouldHaveEventCallback()
    {
        string? receivedContent = null;
        var cut = RenderComponent<FileInput>(parameters => parameters
            .Add(p => p.OnFileSelected, EventCallback.Factory.Create<string>(this, content => receivedContent = content)));

        // Verify the component renders correctly with callback
        cut.Find("input[type='file']").Should().NotBeNull();
    }
}
