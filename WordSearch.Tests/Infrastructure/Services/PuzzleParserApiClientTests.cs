using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using WordSearch.Application.DTOs;
using WordSearch.Infrastructure.Configuration;
using WordSearch.Infrastructure.Services;
using Xunit;

namespace WordSearch.Tests.Infrastructure.Services;

public class PuzzleParserApiClientTests
{
    private readonly ApiSettings _settings;
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;

    public PuzzleParserApiClientTests()
    {
        _settings = new ApiSettings
        {
            CloudflareUrl = "https://test.cloudflare.com/api",
            VercelUrl = "https://test.vercel.com/api"
        };

        _mockHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHandler.Object);
    }

    private PuzzleParserApiClient CreateClient()
    {
        var options = Options.Create(_settings);
        return new PuzzleParserApiClient(_httpClient, options);
    }

    private void SetupMockResponse(HttpStatusCode statusCode, string content)
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnSuccess_WhenApiReturnsValidResponse()
    {
        var responseJson = JsonSerializer.Serialize(new
        {
            success = true,
            puzzleData = "10x10\nABCDEFGHIJ\nTEST",
            message = "Parsed successfully"
        });
        SetupMockResponse(HttpStatusCode.OK, responseJson);

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "cloudflare");

        result.Success.Should().BeTrue();
        result.PuzzleData.Should().Be("10x10\nABCDEFGHIJ\nTEST");
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnFailure_WhenApiReturnsError()
    {
        SetupMockResponse(HttpStatusCode.BadRequest, "Invalid request");

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "cloudflare");

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("BadRequest");
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnFailure_WhenApiReturnsServerError()
    {
        SetupMockResponse(HttpStatusCode.InternalServerError, "Server error");

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "vercel");

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("InternalServerError");
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnFailure_WhenNetworkError()
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "cloudflare");

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("Could not connect");
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnFailure_WhenTimeout()
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("Timeout"));

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "cloudflare");

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("timed out");
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnFailure_WhenInvalidJson()
    {
        SetupMockResponse(HttpStatusCode.OK, "not valid json {{{");

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "cloudflare");

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("Invalid response format");
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnFailure_WhenEmptyResponse()
    {
        SetupMockResponse(HttpStatusCode.OK, "null");

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "base64content",
            ContentType = "image",
            MimeType = "image/png"
        };

        var result = await client.ParseAsync(request, "cloudflare");

        result.Success.Should().BeFalse();
        result.Error.Should().Contain("Empty response");
    }

    [Fact]
    public async Task ParseAsync_ShouldUseCloudflareUrl_WhenCloudfarePlatform()
    {
        SetupMockResponse(HttpStatusCode.OK, JsonSerializer.Serialize(new { success = true, puzzleData = "test" }));

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "test",
            ContentType = "image",
            MimeType = "image/png"
        };

        await client.ParseAsync(request, "cloudflare");

        _mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.ToString().Contains("cloudflare")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task ParseAsync_ShouldUseVercelUrl_WhenVercelPlatform()
    {
        SetupMockResponse(HttpStatusCode.OK, JsonSerializer.Serialize(new { success = true, puzzleData = "test" }));

        var client = CreateClient();
        var request = new ParsePuzzleRequest
        {
            Content = "test",
            ContentType = "image",
            MimeType = "image/png"
        };

        await client.ParseAsync(request, "vercel");

        _mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.ToString().Contains("vercel")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldReturnTrue_WhenApiResponds()
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Options),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

        var client = CreateClient();

        var result = await client.IsAvailableAsync("cloudflare");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldReturnTrue_WhenNoContent()
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Options),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

        var client = CreateClient();

        var result = await client.IsAvailableAsync("cloudflare");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldReturnFalse_WhenApiDown()
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var client = CreateClient();

        var result = await client.IsAvailableAsync("cloudflare");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldReturnFalse_WhenServerError()
    {
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

        var client = CreateClient();

        var result = await client.IsAvailableAsync("cloudflare");

        result.Should().BeFalse();
    }
}
