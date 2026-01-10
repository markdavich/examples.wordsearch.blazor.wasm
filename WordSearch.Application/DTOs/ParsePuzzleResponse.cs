namespace WordSearch.Application.DTOs;

/// <summary>
/// Response from the puzzle parser API.
/// </summary>
public sealed class ParsePuzzleResponse
{
    /// <summary>
    /// Whether the parsing was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// The parsed puzzle data (if successful).
    /// </summary>
    public string? PuzzleData { get; init; }

    /// <summary>
    /// Error message (if unsuccessful).
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// Additional error details.
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Success message with additional info.
    /// </summary>
    public string? Message { get; init; }

    public static ParsePuzzleResponse Successful(string puzzleData, string? message = null) =>
        new() { Success = true, PuzzleData = puzzleData, Message = message };

    public static ParsePuzzleResponse Failed(string error, string? details = null) =>
        new() { Success = false, Error = error, Details = details };
}
