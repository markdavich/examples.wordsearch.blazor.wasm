using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WordSearch.Application.Interfaces;
using WordSearch.Web.Components;
using WordSearch.Web.Services;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class UploadDropdownTests : TestContext
{
    private readonly PuzzleService _puzzleService;

    public UploadDropdownTests()
    {
        _puzzleService = new PuzzleService();
        Services.AddSingleton(_puzzleService);

        // Register mock services for AIUpload
        var mockFileConverter = new Mock<IFileConverter>();
        mockFileConverter.Setup(x => x.IsSupportedFile(It.IsAny<string>())).Returns(true);
        Services.AddSingleton(mockFileConverter.Object);

        var mockPuzzleParser = new Mock<IPuzzleParserApi>();
        Services.AddSingleton(mockPuzzleParser.Object);
    }

    [Fact]
    public void UploadDropdown_ShouldRenderContainer()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".upload-dropdown").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldRenderTriggerButton()
    {
        var cut = RenderComponent<UploadDropdown>();

        var button = cut.Find(".upload-dropdown-trigger");
        button.Should().NotBeNull();
        button.TextContent.Should().Contain("Upload");
    }

    [Fact]
    public void UploadDropdown_ShouldRenderUploadIcon()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".upload-icon").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldRenderDropdownArrow()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".dropdown-arrow").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldNotShowMenu_Initially()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.FindAll(".upload-dropdown-menu").Should().BeEmpty();
    }

    [Fact]
    public void UploadDropdown_ShouldShowMenu_WhenTriggerClicked()
    {
        var cut = RenderComponent<UploadDropdown>();

        var trigger = cut.Find(".upload-dropdown-trigger");
        trigger.Click();

        cut.Find(".upload-dropdown-menu").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldHideMenu_WhenTriggerClickedAgain()
    {
        var cut = RenderComponent<UploadDropdown>();

        var trigger = cut.Find(".upload-dropdown-trigger");
        trigger.Click(); // Open
        trigger.Click(); // Close

        cut.FindAll(".upload-dropdown-menu").Should().BeEmpty();
    }

    [Fact]
    public void UploadDropdown_ShouldRenderFileInput_WhenOpen()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".upload-dropdown-trigger").Click();

        cut.Find(".file-input-container").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldRenderAIUpload_WhenOpen()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".upload-dropdown-trigger").Click();

        cut.Find(".ai-upload-container").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldRenderDivider_WhenOpen()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".upload-dropdown-trigger").Click();

        cut.Find(".dropdown-divider").Should().NotBeNull();
    }

    [Fact]
    public void UploadDropdown_ShouldAddOpenClass_WhenOpen()
    {
        var cut = RenderComponent<UploadDropdown>();

        cut.Find(".upload-dropdown-trigger").Click();

        var arrow = cut.Find(".dropdown-arrow");
        arrow.GetAttribute("class").Should().Contain("open");
    }

    [Fact]
    public void UploadDropdown_ShouldNotHaveOpenClass_Initially()
    {
        var cut = RenderComponent<UploadDropdown>();

        var arrow = cut.Find(".dropdown-arrow");
        arrow.GetAttribute("class").Should().NotContain("open");
    }

    [Fact]
    public async Task UploadDropdown_ShouldCloseMenu_WhenFileSelected()
    {
        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();
        cut.Find(".upload-dropdown-menu").Should().NotBeNull();

        // Simulate file selection through FileInput callback
        var fileInput = cut.FindComponent<FileInput>();
        await cut.InvokeAsync(() => fileInput.Instance.OnFileSelected.InvokeAsync("test content"));

        // Menu should be closed
        cut.FindAll(".upload-dropdown-menu").Should().BeEmpty();
    }

    [Fact]
    public async Task UploadDropdown_ShouldLoadPuzzle_WhenFileSelected()
    {
        string? loadedContent = null;
        _puzzleService.OnPuzzleLoaded += content => loadedContent = content;

        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();

        // Simulate file selection
        var fileInput = cut.FindComponent<FileInput>();
        await cut.InvokeAsync(() => fileInput.Instance.OnFileSelected.InvokeAsync("3x3\nABC\nDEF\nGHI\nTEST"));

        // Verify PuzzleService was called
        loadedContent.Should().Be("3x3\nABC\nDEF\nGHI\nTEST");
    }

    [Fact]
    public async Task UploadDropdown_ShouldCloseMenu_WhenPuzzleParsed()
    {
        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();
        cut.Find(".upload-dropdown-menu").Should().NotBeNull();

        // Simulate AI parsing completion
        var aiUpload = cut.FindComponent<AIUpload>();
        await cut.InvokeAsync(() => aiUpload.Instance.OnPuzzleParsed.InvokeAsync("parsed content"));

        // Menu should be closed
        cut.FindAll(".upload-dropdown-menu").Should().BeEmpty();
    }

    [Fact]
    public async Task UploadDropdown_ShouldLoadPuzzle_WhenPuzzleParsed()
    {
        string? loadedContent = null;
        _puzzleService.OnPuzzleLoaded += content => loadedContent = content;

        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();

        // Simulate AI parsing completion
        var aiUpload = cut.FindComponent<AIUpload>();
        await cut.InvokeAsync(() => aiUpload.Instance.OnPuzzleParsed.InvokeAsync("5x5\nABCDE\nTEST"));

        // Verify PuzzleService was called
        loadedContent.Should().Be("5x5\nABCDE\nTEST");
    }

    [Fact]
    public async Task UploadDropdown_ShouldUpdatePlatform_WhenPlatformChanged()
    {
        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();

        // Simulate platform change
        var aiUpload = cut.FindComponent<AIUpload>();
        await cut.InvokeAsync(() => aiUpload.Instance.OnPlatformChanged.InvokeAsync("vercel"));

        // Verify PuzzleService was updated
        _puzzleService.SelectedPlatform.Should().Be("vercel");
    }

    [Fact]
    public async Task UploadDropdown_ShouldUpdateAiProvider_WhenProviderChanged()
    {
        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();

        // Simulate AI provider change
        var aiUpload = cut.FindComponent<AIUpload>();
        await cut.InvokeAsync(() => aiUpload.Instance.OnAiProviderChanged.InvokeAsync("gemini"));

        // Verify PuzzleService was updated
        _puzzleService.SelectedAiProvider.Should().Be("gemini");
    }

    [Fact]
    public async Task UploadDropdown_ShouldCloseAfterDelay_OnFocusOut()
    {
        var cut = RenderComponent<UploadDropdown>();

        // Open the dropdown
        cut.Find(".upload-dropdown-trigger").Click();
        cut.Find(".upload-dropdown-menu").Should().NotBeNull();

        // Trigger focusout
        var dropdown = cut.Find(".upload-dropdown");
        await cut.InvokeAsync(() => dropdown.TriggerEventAsync("onfocusout", new Microsoft.AspNetCore.Components.Web.FocusEventArgs()));

        // Wait for the delay (200ms + buffer)
        cut.WaitForState(() => cut.FindAll(".upload-dropdown-menu").Count == 0, TimeSpan.FromMilliseconds(400));

        cut.FindAll(".upload-dropdown-menu").Should().BeEmpty();
    }

    [Fact]
    public void UploadDropdown_ShouldPassPlatformToAIUpload()
    {
        _puzzleService.SetPlatform("vercel");

        var cut = RenderComponent<UploadDropdown>();
        cut.Find(".upload-dropdown-trigger").Click();

        var aiUpload = cut.FindComponent<AIUpload>();
        aiUpload.Instance.Platform.Should().Be("vercel");
    }

    [Fact]
    public void UploadDropdown_ShouldPassAiProviderToAIUpload()
    {
        _puzzleService.SetAiProvider("together");

        var cut = RenderComponent<UploadDropdown>();
        cut.Find(".upload-dropdown-trigger").Click();

        var aiUpload = cut.FindComponent<AIUpload>();
        aiUpload.Instance.AiProvider.Should().Be("together");
    }

    [Fact]
    public void UploadDropdown_ShouldHaveTabIndexForFocusability()
    {
        var cut = RenderComponent<UploadDropdown>();

        var dropdown = cut.Find(".upload-dropdown");
        dropdown.GetAttribute("tabindex").Should().Be("-1");
    }

    [Fact]
    public void UploadDropdown_ShouldRenderWordSearchFileLabel()
    {
        var cut = RenderComponent<UploadDropdown>();
        cut.Find(".upload-dropdown-trigger").Click();

        var fileInput = cut.FindComponent<FileInput>();
        fileInput.Instance.Label.Should().Be("Word Search File");
    }
}
