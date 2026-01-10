using FluentAssertions;
using WordSearch.Web.Services;
using Xunit;

namespace WordSearch.Tests.Web.Services;

public class PuzzleServiceTests
{
    [Fact]
    public void PuzzleService_ShouldHaveDefaultPlatform()
    {
        var service = new PuzzleService();

        service.SelectedPlatform.Should().Be("cloudflare");
    }

    [Fact]
    public void PuzzleService_ShouldHaveDefaultAiProvider()
    {
        var service = new PuzzleService();

        service.SelectedAiProvider.Should().Be("groq");
    }

    [Fact]
    public void LoadPuzzle_ShouldFireOnPuzzleLoaded()
    {
        var service = new PuzzleService();
        string? receivedContent = null;
        service.OnPuzzleLoaded += content => receivedContent = content;

        service.LoadPuzzle("test content");

        receivedContent.Should().Be("test content");
    }

    [Fact]
    public void LoadPuzzle_ShouldNotThrow_WhenNoSubscribers()
    {
        var service = new PuzzleService();

        var act = () => service.LoadPuzzle("test content");

        act.Should().NotThrow();
    }

    [Fact]
    public void SetPlatform_ShouldUpdateSelectedPlatform()
    {
        var service = new PuzzleService();

        service.SetPlatform("vercel");

        service.SelectedPlatform.Should().Be("vercel");
    }

    [Fact]
    public void SetPlatform_ShouldFireOnPlatformChanged()
    {
        var service = new PuzzleService();
        string? receivedPlatform = null;
        service.OnPlatformChanged += platform => receivedPlatform = platform;

        service.SetPlatform("vercel");

        receivedPlatform.Should().Be("vercel");
    }

    [Fact]
    public void SetAiProvider_ShouldUpdateSelectedAiProvider()
    {
        var service = new PuzzleService();

        service.SetAiProvider("gemini");

        service.SelectedAiProvider.Should().Be("gemini");
    }

    [Fact]
    public void SetAiProvider_ShouldFireOnAiProviderChanged()
    {
        var service = new PuzzleService();
        string? receivedProvider = null;
        service.OnAiProviderChanged += provider => receivedProvider = provider;

        service.SetAiProvider("gemini");

        receivedProvider.Should().Be("gemini");
    }

    [Fact]
    public void SetPlatform_ShouldResetCloudflareAi_WhenSwitchingFromCloudflare()
    {
        var service = new PuzzleService();
        service.SetAiProvider("cloudflare-ai");

        service.SetPlatform("vercel");

        service.SelectedAiProvider.Should().Be("groq");
    }

    [Fact]
    public void SetPlatform_ShouldNotResetProvider_WhenNotCloudflareAi()
    {
        var service = new PuzzleService();
        service.SetAiProvider("gemini");

        service.SetPlatform("vercel");

        service.SelectedAiProvider.Should().Be("gemini");
    }

    [Fact]
    public void SetPlatform_ShouldFireAiProviderChanged_WhenResettingCloudflareAi()
    {
        var service = new PuzzleService();
        service.SetAiProvider("cloudflare-ai");

        string? receivedProvider = null;
        service.OnAiProviderChanged += provider => receivedProvider = provider;

        service.SetPlatform("vercel");

        receivedProvider.Should().Be("groq");
    }

    [Fact]
    public void SetPlatform_ShouldNotFireAiProviderChanged_WhenNotResetting()
    {
        var service = new PuzzleService();
        service.SetAiProvider("gemini");

        string? receivedProvider = null;
        service.OnAiProviderChanged += provider => receivedProvider = provider;

        service.SetPlatform("vercel");

        receivedProvider.Should().BeNull();
    }

    [Fact]
    public void MultipleSubscribers_ShouldAllReceiveEvents()
    {
        var service = new PuzzleService();
        string? received1 = null;
        string? received2 = null;
        service.OnPuzzleLoaded += content => received1 = content;
        service.OnPuzzleLoaded += content => received2 = content;

        service.LoadPuzzle("test");

        received1.Should().Be("test");
        received2.Should().Be("test");
    }

    [Fact]
    public void UnsubscribedHandler_ShouldNotReceiveEvents()
    {
        var service = new PuzzleService();
        string? received = null;
        void Handler(string content) => received = content;

        service.OnPuzzleLoaded += Handler;
        service.OnPuzzleLoaded -= Handler;

        service.LoadPuzzle("test");

        received.Should().BeNull();
    }
}
