# Application Layer

The Application layer defines interfaces for external capabilities and provides services that orchestrate domain logic.

## What Belongs Here

- **Interfaces**: Contracts for external services (e.g., `IAiParserService`)
- **DTOs**: Data transfer objects for crossing boundaries
- **Simple Services**: Lightweight orchestration logic
- **Use Case Classes**: If you have complex multi-step operations

## What Does NOT Belong Here

- Implementations of external services (that's Infrastructure)
- UI logic or Blazor components (that's Web)
- Core business algorithms (that's Domain)
- Direct HTTP calls or file I/O

## Key Interfaces

### IAiParserService

Defines the contract for parsing puzzle images using AI.

```csharp
public interface IAiParserService
{
    Task<AiParseResult> ParseImageAsync(
        byte[] imageData,
        string platform,
        string aiProvider);
}
```

**Why an interface?**

The Application layer needs to parse images but shouldn't know HOW (which API, which provider). The Infrastructure layer provides the actual implementation.

### IAiPlatformClient

Defines the contract for communicating with a specific cloud platform.

```csharp
public interface IAiPlatformClient
{
    string PlatformName { get; }
    Task<string> ParseImageAsync(byte[] imageData, string aiProvider);
}
```

**Multiple implementations:**
- `CloudflareWorkerClient` (Infrastructure)
- `VercelFunctionClient` (Infrastructure)

## DTOs and Results

### AiParseResult

Returned from AI parsing operations.

```csharp
public class AiParseResult
{
    public bool Success { get; }
    public string? PuzzleText { get; }
    public string? ErrorMessage { get; }
    public string Platform { get; }
    public string AiProvider { get; }
}
```

**Why a result object instead of exceptions?**

AI parsing can fail for many expected reasons (bad image, rate limits, invalid format). Using a result object makes success/failure explicit and avoids exception overhead.

## Services

### PuzzleService

A singleton service for cross-component communication in the Web layer.

```csharp
public class PuzzleService
{
    public event Action<string>? OnPuzzleLoaded;
    public string SelectedPlatform { get; }
    public string SelectedAiProvider { get; }

    public void LoadPuzzle(string content);
    public void SetPlatform(string platform);
    public void SetAiProvider(string provider);
}
```

**Why events?**

Blazor components in the navbar need to notify the main page when a puzzle is loaded. Events allow loose coupling—the navbar doesn't need a reference to the page.

## Layer Boundaries

The Application layer acts as a boundary between the inner Domain and outer Infrastructure/Web layers.

```
Web ──calls──> Application ──uses──> Domain
                   │
                   └──defines interfaces──> Infrastructure implements
```

**Flow example:**

1. User uploads an image in Web layer
2. Web calls `IAiParserService.ParseImageAsync()`
3. Infrastructure's `AiParserService` calls external API
4. Result flows back to Web
5. Web creates `Puzzle` (Domain) from the result

## Design Decisions

### Why separate Application from Infrastructure?

**Testability**: We can test Application logic with mock implementations of `IAiParserService`.

**Flexibility**: We can swap AI providers without touching Application code.

**Clarity**: Looking at Application shows WHAT the system can do. Looking at Infrastructure shows HOW.

### When to add a service vs. putting logic in components?

Add a service when:
- Multiple components need the same logic
- The logic involves coordinating multiple operations
- You need to share state across components

Keep logic in components when:
- It's purely UI-related
- Only one component needs it
- It's simple and self-contained
