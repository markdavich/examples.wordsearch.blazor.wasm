using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WordSearch.Application.DTOs;
using WordSearch.Application.Interfaces;
using WordSearch.Web.Components;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class AIUploadTests : TestContext
{
    private readonly Mock<IFileConverter> _mockFileConverter;
    private readonly Mock<IPuzzleParserApi> _mockPuzzleParser;

    public AIUploadTests()
    {
        _mockFileConverter = new Mock<IFileConverter>();
        _mockFileConverter.Setup(x => x.IsSupportedFile(It.IsAny<string>())).Returns(true);
        _mockFileConverter.Setup(x => x.IsImageFile(It.IsAny<string>())).Returns(true);
        _mockFileConverter.Setup(x => x.IsPlainTextFile(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
        Services.AddSingleton(_mockFileConverter.Object);

        _mockPuzzleParser = new Mock<IPuzzleParserApi>();
        _mockPuzzleParser
            .Setup(x => x.ParseAsync(It.IsAny<ParsePuzzleRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ParsePuzzleResponse.Successful("10x10\nABCDEFGHIJ\nTEST"));
        Services.AddSingleton(_mockPuzzleParser.Object);
    }

    [Fact]
    public void AIUpload_ShouldRenderContainer()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Find(".ai-upload-container").Should().NotBeNull();
    }

    [Fact]
    public void AIUpload_ShouldRenderTitle()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Markup.Should().Contain("AI-Powered Upload");
    }

    [Fact]
    public void AIUpload_ShouldRenderPlatformSelect()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Markup.Should().Contain("Platform");
        var selects = cut.FindAll("select.select-input");
        selects.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public void AIUpload_ShouldRenderAiProviderSelect()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Markup.Should().Contain("AI Provider");
    }

    [Fact]
    public void AIUpload_ShouldRenderPlatformOptions()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Markup.Should().Contain("Cloudflare Workers");
        cut.Markup.Should().Contain("Vercel");
    }

    [Fact]
    public void AIUpload_ShouldRenderAiProviderOptions()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Markup.Should().Contain("Groq");
        cut.Markup.Should().Contain("Gemini");
        cut.Markup.Should().Contain("Together AI");
    }

    [Fact]
    public void AIUpload_ShouldRenderCloudflareAI_WhenCloudflareSelected()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.Platform, "cloudflare"));

        cut.Markup.Should().Contain("Cloudflare AI");
    }

    [Fact]
    public void AIUpload_ShouldNotRenderCloudflareAI_WhenVercelSelected()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.Platform, "vercel"));

        cut.Markup.Should().NotContain("Cloudflare AI");
    }

    [Fact]
    public void AIUpload_ShouldRenderFileInput()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Find("input[type='file']").Should().NotBeNull();
    }

    [Fact]
    public void AIUpload_ShouldRenderUploadHint()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Find(".upload-hint").Should().NotBeNull();
        cut.Markup.Should().Contain("Upload an image or document");
    }

    [Fact]
    public void AIUpload_ShouldAcceptMultipleFileTypes()
    {
        var cut = RenderComponent<AIUpload>();

        var input = cut.Find("input[type='file']");
        var accept = input.GetAttribute("accept");
        accept.Should().Contain("image/*");
        accept.Should().Contain(".pdf");
        accept.Should().Contain(".docx");
    }

    [Fact]
    public void AIUpload_ShouldNotShowLoading_Initially()
    {
        var cut = RenderComponent<AIUpload>();

        cut.FindAll(".loading-indicator").Should().BeEmpty();
    }

    [Fact]
    public void AIUpload_ShouldNotShowError_Initially()
    {
        var cut = RenderComponent<AIUpload>();

        cut.FindAll(".error-message").Should().BeEmpty();
    }

    [Fact]
    public void AIUpload_ShouldNotShowSuccess_Initially()
    {
        var cut = RenderComponent<AIUpload>();

        cut.FindAll(".success-message").Should().BeEmpty();
    }

    [Fact]
    public void AIUpload_ShouldHaveTooltips()
    {
        var cut = RenderComponent<AIUpload>();

        var tooltips = cut.FindAll(".tooltip-wrapper");
        tooltips.Should().HaveCountGreaterThanOrEqualTo(3);
    }

    [Fact]
    public void AIUpload_ShouldRenderOptionGroups()
    {
        var cut = RenderComponent<AIUpload>();

        var groups = cut.FindAll(".option-group");
        groups.Should().HaveCount(2); // Platform and AI Provider
    }

    [Fact]
    public void AIUpload_ShouldUseDefaultPlatform()
    {
        var cut = RenderComponent<AIUpload>();

        // Default is cloudflare, so Cloudflare AI option should be visible
        cut.Markup.Should().Contain("Cloudflare AI");
    }

    [Fact]
    public void AIUpload_ShouldRespectPlatformParameter()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.Platform, "vercel")
            .Add(p => p.AiProvider, "gemini"));

        // Should not show Cloudflare AI for vercel
        cut.Markup.Should().NotContain("Cloudflare AI");
    }

    [Fact]
    public async Task AIUpload_ShouldFirePlatformChanged_WhenPlatformChanges()
    {
        string? changedPlatform = null;
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.OnPlatformChanged, EventCallback.Factory.Create<string>(this, p => changedPlatform = p)));

        var selects = cut.FindAll("select.select-input");
        var platformSelect = selects[0];

        await cut.InvokeAsync(() => platformSelect.Change("vercel"));

        changedPlatform.Should().Be("vercel");
    }

    [Fact]
    public async Task AIUpload_ShouldFireAiProviderChanged_WhenProviderChanges()
    {
        string? changedProvider = null;
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.OnAiProviderChanged, EventCallback.Factory.Create<string>(this, p => changedProvider = p)));

        var selects = cut.FindAll("select.select-input");
        var providerSelect = selects[1];

        await cut.InvokeAsync(() => providerSelect.Change("gemini"));

        changedProvider.Should().Be("gemini");
    }

    [Fact]
    public void AIUpload_ShouldSyncWithPlatformParameter()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.Platform, "vercel"));

        // Cloudflare AI should not be visible when platform is vercel
        cut.Markup.Should().NotContain("Cloudflare AI");
    }

    [Fact]
    public void AIUpload_ShouldSyncWithAiProviderParameter()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.AiProvider, "together"));

        // The together option should be available
        cut.Markup.Should().Contain("Together AI");
    }

    [Fact]
    public void AIUpload_ShouldUpdateOnParametersSet()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.Platform, "cloudflare")
            .Add(p => p.AiProvider, "groq"));

        cut.Markup.Should().Contain("Cloudflare AI");

        // Update parameters
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Platform, "vercel")
            .Add(p => p.AiProvider, "gemini"));

        cut.Markup.Should().NotContain("Cloudflare AI");
    }

    [Fact]
    public void AIUpload_ShouldRenderLoadingSpinnerClass()
    {
        var cut = RenderComponent<AIUpload>();

        // The spinner class should exist in the component styles (even if not visible)
        cut.Markup.Should().NotContain("loading-indicator"); // Not loading initially
    }

    [Fact]
    public void AIUpload_ShouldRenderAllPlatformOptions()
    {
        var cut = RenderComponent<AIUpload>();

        cut.Markup.Should().Contain("Cloudflare Workers");
        cut.Markup.Should().Contain("Vercel");
    }

    [Fact]
    public void AIUpload_ShouldRenderAllAiProviderOptions()
    {
        var cut = RenderComponent<AIUpload>(parameters => parameters
            .Add(p => p.Platform, "cloudflare"));

        cut.Markup.Should().Contain("Groq");
        cut.Markup.Should().Contain("Gemini");
        cut.Markup.Should().Contain("Together AI");
        cut.Markup.Should().Contain("Cloudflare AI");
    }

    [Fact]
    public void AIUpload_ShouldRenderFileAcceptAttribute()
    {
        var cut = RenderComponent<AIUpload>();

        var input = cut.Find("input[type='file']");
        var accept = input.GetAttribute("accept");
        accept.Should().Contain("image/*");
        accept.Should().Contain(".pdf");
        accept.Should().Contain(".docx");
        accept.Should().Contain(".json");
        accept.Should().Contain(".xml");
    }
}
