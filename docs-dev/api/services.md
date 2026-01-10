# Services

Services contain application logic that orchestrates domain objects and external operations.

## Application Layer Services

### WordFinderService

Finds words in a grid by searching all directions.

**File:** `WordSearch.Application/Services/WordFinderService.cs`

**Implements:** `IWordFinder`

```csharp
public sealed class WordFinderService : IWordFinder
{
    public IEnumerable<Match> FindWords(char[,] grid, IEnumerable<string> words);
}
```

**How it works:**

The service aggregates three specialized finders:
- `HorizontalWordFinder` - searches left-to-right and right-to-left
- `VerticalWordFinder` - searches top-to-bottom and bottom-to-top
- `DiagonalWordFinder` - searches all four diagonal directions

```csharp
// Usage
var finder = new WordFinderService();
var matches = finder.FindWords(puzzle.Letters, puzzle.Words);

foreach (var match in matches)
{
    Console.WriteLine($"Found {match.Word} from {match.Start} to {match.End}");
}
```

---

### HorizontalWordFinder

Searches for words horizontally (left-right and right-left).

**File:** `WordSearch.Application/Services/HorizontalWordFinder.cs`

**Implements:** `IDirectionalFinder`

```csharp
public sealed class HorizontalWordFinder : IDirectionalFinder
{
    public IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words);
}
```

---

### VerticalWordFinder

Searches for words vertically (top-down and bottom-up).

**File:** `WordSearch.Application/Services/VerticalWordFinder.cs`

**Implements:** `IDirectionalFinder`

```csharp
public sealed class VerticalWordFinder : IDirectionalFinder
{
    public IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words);
}
```

---

### DiagonalWordFinder

Searches for words diagonally in all four directions.

**File:** `WordSearch.Application/Services/DiagonalWordFinder.cs`

**Implements:** `IDirectionalFinder`

```csharp
public sealed class DiagonalWordFinder : IDirectionalFinder
{
    public IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words);
}
```

---

### MatrixService

Utility service for matrix operations.

**File:** `WordSearch.Application/Services/MatrixService.cs`

```csharp
public static class MatrixService
{
    public static MatrixShape GetShape(int rows, int cols);
    public static string GetRow(char[,] matrix, int rowIndex);
    public static string GetColumn(char[,] matrix, int colIndex);
    public static string GetDiagonal(char[,] matrix, int startRow, int startCol, DiagonalDirection direction);
}
```

---

## Web Layer Services

### PuzzleService

Singleton service for cross-component communication.

**File:** `WordSearch.Web/Services/PuzzleService.cs`

```csharp
public class PuzzleService
{
    // Events
    public event Action<string>? OnPuzzleLoaded;
    public event Action<string>? OnPlatformChanged;
    public event Action<string>? OnAiProviderChanged;

    // State
    public string SelectedPlatform { get; }
    public string SelectedAiProvider { get; }

    // Methods
    public void LoadPuzzle(string content);
    public void SetPlatform(string platform);
    public void SetAiProvider(string provider);
}
```

| Property | Default | Description |
|----------|---------|-------------|
| `SelectedPlatform` | `"cloudflare"` | Current AI platform |
| `SelectedAiProvider` | `"groq"` | Current AI provider |

**Events:**

| Event | Fires When |
|-------|------------|
| `OnPuzzleLoaded` | Puzzle content is loaded |
| `OnPlatformChanged` | Platform selection changes |
| `OnAiProviderChanged` | AI provider selection changes |

**Usage Pattern:**

```csharp
// In a component that loads puzzles (e.g., ExampleSelector)
@inject PuzzleService PuzzleService

private void LoadPuzzle(string content)
{
    PuzzleService.LoadPuzzle(content);  // Fire event
}

// In a component that displays puzzles (e.g., Index page)
@inject PuzzleService PuzzleService

protected override void OnInitialized()
{
    PuzzleService.OnPuzzleLoaded += HandlePuzzleLoaded;
}

private void HandlePuzzleLoaded(string content)
{
    // Parse and display the puzzle
    StateHasChanged();
}

public void Dispose()
{
    PuzzleService.OnPuzzleLoaded -= HandlePuzzleLoaded;
}
```

---

## Interfaces

### IWordFinder

Contract for word finding services.

**File:** `WordSearch.Application/Interfaces/IWordFinder.cs`

```csharp
public interface IWordFinder
{
    IEnumerable<Match> FindWords(char[,] grid, IEnumerable<string> words);
}
```

---

### IDirectionalFinder

Contract for direction-specific word finders.

**File:** `WordSearch.Application/Interfaces/IDirectionalFinder.cs`

```csharp
public interface IDirectionalFinder
{
    IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words);
}
```

---

### IPuzzleParserApi

Contract for AI-powered puzzle parsing.

**File:** `WordSearch.Application/Interfaces/IPuzzleParserApi.cs`

```csharp
public interface IPuzzleParserApi
{
    Task<ParsePuzzleResponse> ParseAsync(
        ParsePuzzleRequest request,
        string platform,
        CancellationToken cancellationToken = default);

    Task<bool> IsAvailableAsync(
        string platform,
        CancellationToken cancellationToken = default);
}
```

---

## DTOs

### ParsePuzzleRequest

Request to parse a puzzle from file content.

**File:** `WordSearch.Application/DTOs/ParsePuzzleRequest.cs`

```csharp
public sealed class ParsePuzzleRequest
{
    public required string Content { get; init; }
    public required string ContentType { get; init; }
    public required string MimeType { get; init; }
    public string AiProvider { get; init; } = "groq";
}
```

| Property | Description |
|----------|-------------|
| `Content` | Base64 for images, plain text for txt |
| `ContentType` | `"image"` or `"text"` |
| `MimeType` | e.g., `"image/png"`, `"text/plain"` |
| `AiProvider` | `"groq"`, `"gemini"`, `"together"`, `"cloudflare-ai"` |

---

### ParsePuzzleResponse

Response from the puzzle parser API.

**File:** `WordSearch.Application/DTOs/ParsePuzzleResponse.cs`

```csharp
public sealed class ParsePuzzleResponse
{
    public bool Success { get; init; }
    public string? PuzzleData { get; init; }
    public string? Error { get; init; }
    public string? Details { get; init; }
    public string? Message { get; init; }

    public static ParsePuzzleResponse Successful(string puzzleData, string? message = null);
    public static ParsePuzzleResponse Failed(string error, string? details = null);
}
```
