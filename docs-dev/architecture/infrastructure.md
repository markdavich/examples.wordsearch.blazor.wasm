# Infrastructure Layer

The Infrastructure layer contains implementations of external services defined by Application interfaces.

## What Belongs Here

- **HTTP Clients**: Calling external APIs
- **Platform Clients**: Cloudflare Workers, Vercel Functions
- **File System Operations**: If any
- **External Service Wrappers**: Any third-party integrations

## What Does NOT Belong Here

- Core business logic (that's Domain)
- Interface definitions (that's Application)
- UI code (that's Web)

## Key Implementations

### AiParserService

Implements `IAiParserService` by delegating to platform-specific clients.

```csharp
public class AiParserService : IAiParserService
{
    private readonly IEnumerable<IAiPlatformClient> _clients;

    public async Task<AiParseResult> ParseImageAsync(
        byte[] imageData,
        string platform,
        string aiProvider)
    {
        var client = _clients.FirstOrDefault(c =>
            c.PlatformName == platform);

        if (client == null)
            return AiParseResult.Failure($"Unknown platform: {platform}");

        var result = await client.ParseImageAsync(imageData, aiProvider);
        return AiParseResult.Success(result, platform, aiProvider);
    }
}
```

### CloudflareWorkerClient

Calls a Cloudflare Worker to parse images.

```csharp
public class CloudflareWorkerClient : IAiPlatformClient
{
    public string PlatformName => "cloudflare";
    private readonly HttpClient _httpClient;
    private readonly string _workerUrl;

    public async Task<string> ParseImageAsync(
        byte[] imageData,
        string aiProvider)
    {
        var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(imageData), "image", "puzzle.png");
        content.Add(new StringContent(aiProvider), "provider");

        var response = await _httpClient.PostAsync(_workerUrl, content);
        return await response.Content.ReadAsStringAsync();
    }
}
```

### VercelFunctionClient

Calls a Vercel serverless function to parse images.

```csharp
public class VercelFunctionClient : IAiPlatformClient
{
    public string PlatformName => "vercel";
    // Similar implementation to CloudflareWorkerClient
}
```

## Configuration

Platform URLs are configured in `appsettings.json`:

```json
{
  "AiParsing": {
    "CloudflareWorkerUrl": "https://puzzle-parser.workers.dev",
    "VercelFunctionUrl": "https://puzzle-parser.vercel.app/api/parse"
  }
}
```

## Error Handling

Infrastructure code must handle external failures gracefully:

```csharp
try
{
    var response = await _httpClient.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
        return AiParseResult.Failure(
            $"API returned {response.StatusCode}");
    }

    return AiParseResult.Success(await response.Content.ReadAsStringAsync());
}
catch (HttpRequestException ex)
{
    return AiParseResult.Failure($"Network error: {ex.Message}");
}
catch (TaskCanceledException)
{
    return AiParseResult.Failure("Request timed out");
}
```

## Dependency Registration

Infrastructure services are registered in `Program.cs`:

```csharp
// Register platform clients
builder.Services.AddHttpClient<CloudflareWorkerClient>();
builder.Services.AddHttpClient<VercelFunctionClient>();

// Register as collection for IAiParserService
builder.Services.AddSingleton<IAiPlatformClient, CloudflareWorkerClient>();
builder.Services.AddSingleton<IAiPlatformClient, VercelFunctionClient>();

// Register the main service
builder.Services.AddSingleton<IAiParserService, AiParserService>();
```

## Design Decisions

### Why multiple platform clients?

**Redundancy**: If Cloudflare has issues, users can switch to Vercel.

**Flexibility**: Different platforms may have different rate limits or features.

**Cost**: Users can choose based on their own cloud provider preferences.

### Why not call AI APIs directly from the browser?

**Security**: API keys would be exposed in browser JavaScript/WebAssembly.

**CORS**: Most AI APIs don't allow browser requests.

**Rate Limiting**: Easier to control from a server.

The serverless functions (Cloudflare Workers, Vercel Functions) act as secure proxies that hold the API keys.
