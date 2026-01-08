using WordSearchSolver.Application.Interfaces;

namespace WordSearchSolver.Infrastructure.Services;

/// <summary>
/// Converts files to appropriate formats for API upload.
/// </summary>
public sealed class FileConverterService : IFileConverter
{
    private static readonly HashSet<string> ImageTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/png",
        "image/jpeg",
        "image/jpg",
        "image/webp",
        "image/gif",
        "image/bmp",
        "image/tiff"
    };

    private static readonly HashSet<string> DocumentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Text formats
        "text/plain",
        "text/csv",
        "text/html",
        "application/json",
        // Office formats
        "application/pdf",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // .docx
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // .xlsx
        "application/msword", // .doc
        "application/vnd.ms-excel", // .xls
        // Other
        "application/xml",
        "text/xml"
    };

    private static readonly HashSet<string> PlainTextTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "text/plain",
        "text/csv",
        "text/html",
        "application/json",
        "application/xml",
        "text/xml"
    };

    private static readonly HashSet<string> PlainTextExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".txt", ".csv", ".html", ".htm", ".json", ".xml", ".md", ".markdown"
    };

    public async Task<string> ToBase64Async(Stream fileStream)
    {
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public async Task<string> ReadTextAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync();
    }

    public bool IsPlainTextFile(string mimeType, string fileName)
    {
        if (PlainTextTypes.Contains(mimeType))
            return true;

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return PlainTextExtensions.Contains(extension);
    }

    public bool IsImageFile(string mimeType)
    {
        return ImageTypes.Contains(mimeType);
    }

    public bool IsSupportedFile(string mimeType)
    {
        return ImageTypes.Contains(mimeType) || DocumentTypes.Contains(mimeType);
    }
}
