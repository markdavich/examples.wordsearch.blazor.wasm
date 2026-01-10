using WordSearch.Application.DTOs;

namespace WordSearch.Application.Interfaces;

/// <summary>
/// Interface for the AI-powered puzzle parser API.
/// </summary>
public interface IPuzzleParserApi
{
    /// <summary>
    /// Parses a puzzle from an image or document using AI.
    /// </summary>
    /// <param name="request">The parse request with file content.</param>
    /// <param name="platform">The platform to use (cloudflare or vercel).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The parsed puzzle data or error.</returns>
    Task<ParsePuzzleResponse> ParseAsync(
        ParsePuzzleRequest request,
        string platform,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Checks if the API is available.
    /// </summary>
    Task<bool> IsAvailableAsync(string platform, CancellationToken cancellationToken = default);
}
