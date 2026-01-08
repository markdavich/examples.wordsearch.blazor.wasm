namespace WordSearchSolver.Application.DTOs;

/// <summary>
/// Request to parse a puzzle from file content.
/// </summary>
public sealed class ParsePuzzleRequest
{
    /// <summary>
    /// The file content (base64 for images/PDFs, plain text for txt/csv).
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// The type of content: "image" or "text".
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// The MIME type of the original file.
    /// </summary>
    public required string MimeType { get; init; }

    /// <summary>
    /// The AI provider to use (groq, gemini, together, cloudflare-ai).
    /// </summary>
    public string AiProvider { get; init; } = "groq";
}
