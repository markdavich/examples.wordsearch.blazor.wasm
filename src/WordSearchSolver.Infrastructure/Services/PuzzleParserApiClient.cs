using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WordSearchSolver.Application.DTOs;
using WordSearchSolver.Application.Interfaces;
using WordSearchSolver.Infrastructure.Configuration;

namespace WordSearchSolver.Infrastructure.Services;

/// <summary>
/// HTTP client for the puzzle parser API.
/// </summary>
public sealed class PuzzleParserApiClient : IPuzzleParserApi
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _settings;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public PuzzleParserApiClient(HttpClient httpClient, IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<ParsePuzzleResponse> ParseAsync(
        ParsePuzzleRequest request,
        string platform,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = _settings.GetUrl(platform);
            var response = await _httpClient.PostAsJsonAsync(url, request, JsonOptions, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return ParsePuzzleResponse.Failed(
                    $"API returned {response.StatusCode}",
                    errorContent
                );
            }

            var result = await response.Content.ReadFromJsonAsync<ParsePuzzleResponse>(JsonOptions, cancellationToken);
            return result ?? ParsePuzzleResponse.Failed("Empty response from API");
        }
        catch (HttpRequestException ex)
        {
            return ParsePuzzleResponse.Failed(
                $"Could not connect to {platform} API",
                ex.Message
            );
        }
        catch (TaskCanceledException)
        {
            return ParsePuzzleResponse.Failed("Request timed out");
        }
        catch (JsonException ex)
        {
            return ParsePuzzleResponse.Failed(
                "Invalid response format",
                ex.Message
            );
        }
    }

    public async Task<bool> IsAvailableAsync(string platform, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = _settings.GetUrl(platform);
            var request = new HttpRequestMessage(HttpMethod.Options, url);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch
        {
            return false;
        }
    }
}
