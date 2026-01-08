namespace WordSearchSolver.Application.Interfaces;

/// <summary>
/// Interface for converting files to appropriate formats for API upload.
/// </summary>
public interface IFileConverter
{
    /// <summary>
    /// Converts a file to base64 string.
    /// </summary>
    Task<string> ToBase64Async(Stream fileStream);

    /// <summary>
    /// Reads a text file as string.
    /// </summary>
    Task<string> ReadTextAsync(Stream fileStream);

    /// <summary>
    /// Determines if the file should be read as plain text.
    /// </summary>
    bool IsPlainTextFile(string mimeType, string fileName);

    /// <summary>
    /// Determines if the file is an image.
    /// </summary>
    bool IsImageFile(string mimeType);

    /// <summary>
    /// Determines if the file type is supported for AI parsing.
    /// </summary>
    bool IsSupportedFile(string mimeType);
}
