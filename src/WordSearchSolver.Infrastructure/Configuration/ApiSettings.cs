namespace WordSearchSolver.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for the puzzle parser API.
/// </summary>
public sealed class ApiSettings
{
    /// <summary>
    /// The section name in configuration.
    /// </summary>
    public const string SectionName = "PuzzleParserApi";

    /// <summary>
    /// URL for the Cloudflare Workers API.
    /// </summary>
    public string CloudflareUrl { get; set; } = "https://puzzle-parser-api.markdavich.workers.dev";

    /// <summary>
    /// URL for the Vercel API.
    /// </summary>
    public string VercelUrl { get; set; } = "https://examples-wordsearch.vercel.app/api/puzzle-parser";

    /// <summary>
    /// Gets the URL for the specified platform.
    /// </summary>
    public string GetUrl(string platform) => platform.ToLowerInvariant() switch
    {
        "cloudflare" => CloudflareUrl,
        "vercel" => VercelUrl,
        _ => throw new ArgumentException($"Unknown platform: {platform}")
    };
}
