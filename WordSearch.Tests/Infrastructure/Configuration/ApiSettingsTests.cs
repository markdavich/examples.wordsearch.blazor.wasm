using FluentAssertions;
using WordSearch.Infrastructure.Configuration;

namespace WordSearch.Tests.Infrastructure.Configuration;

public class ApiSettingsTests
{
    [Fact]
    public void SectionName_ShouldBePuzzleParserApi()
    {
        ApiSettings.SectionName.Should().Be("PuzzleParserApi");
    }

    [Fact]
    public void CloudflareUrl_ShouldHaveDefaultValue()
    {
        var settings = new ApiSettings();

        settings.CloudflareUrl.Should().NotBeNullOrEmpty();
        settings.CloudflareUrl.Should().StartWith("https://");
    }

    [Fact]
    public void VercelUrl_ShouldHaveDefaultValue()
    {
        var settings = new ApiSettings();

        settings.VercelUrl.Should().NotBeNullOrEmpty();
        settings.VercelUrl.Should().StartWith("https://");
    }

    [Fact]
    public void GetUrl_ShouldReturnCloudflareUrl_ForCloudfarePlatform()
    {
        var settings = new ApiSettings
        {
            CloudflareUrl = "https://cloudflare.example.com"
        };

        var url = settings.GetUrl("cloudflare");

        url.Should().Be("https://cloudflare.example.com");
    }

    [Fact]
    public void GetUrl_ShouldReturnVercelUrl_ForVercelPlatform()
    {
        var settings = new ApiSettings
        {
            VercelUrl = "https://vercel.example.com"
        };

        var url = settings.GetUrl("vercel");

        url.Should().Be("https://vercel.example.com");
    }

    [Fact]
    public void GetUrl_ShouldBeCaseInsensitive()
    {
        var settings = new ApiSettings
        {
            CloudflareUrl = "https://cloudflare.example.com",
            VercelUrl = "https://vercel.example.com"
        };

        settings.GetUrl("CLOUDFLARE").Should().Be("https://cloudflare.example.com");
        settings.GetUrl("Cloudflare").Should().Be("https://cloudflare.example.com");
        settings.GetUrl("VERCEL").Should().Be("https://vercel.example.com");
        settings.GetUrl("Vercel").Should().Be("https://vercel.example.com");
    }

    [Fact]
    public void GetUrl_ShouldThrowException_ForUnknownPlatform()
    {
        var settings = new ApiSettings();

        var action = () => settings.GetUrl("unknown");

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Unknown platform: unknown*");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("aws")]
    [InlineData("azure")]
    [InlineData("")]
    public void GetUrl_ShouldThrowException_ForInvalidPlatforms(string platform)
    {
        var settings = new ApiSettings();

        var action = () => settings.GetUrl(platform);

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        var settings = new ApiSettings
        {
            CloudflareUrl = "https://custom-cloudflare.com",
            VercelUrl = "https://custom-vercel.com"
        };

        settings.CloudflareUrl.Should().Be("https://custom-cloudflare.com");
        settings.VercelUrl.Should().Be("https://custom-vercel.com");
    }
}
