# Domain Types

The Domain layer contains all core business objects. These types have no external dependencies.

## Entities

Entities are objects with identity that persist over time.

### WordSearchPuzzle

The main entity representing a word search puzzle.

**File:** `WordSearch.Domain/Entities/WordSearchPuzzle.cs`

```csharp
public sealed class WordSearchPuzzle
{
    public Dimensions Dimensions { get; }
    public char[,] Letters { get; }
    public IReadOnlyList<string> Words { get; }

    public static WordSearchPuzzle Parse(string fileContent);
}
```

| Property | Type | Description |
|----------|------|-------------|
| `Dimensions` | `Dimensions` | Grid size (rows x columns) |
| `Letters` | `char[,]` | 2D array of uppercase letters |
| `Words` | `IReadOnlyList<string>` | Words to find (uppercase) |

**Static Methods:**

| Method | Description |
|--------|-------------|
| `Parse(string)` | Creates a puzzle from file content |

**File Format:**
```
10x10
A B C D E F G H I J
K L M N O P Q R S T
...
WORD1
WORD2
```

---

### Match

Represents a found word in the grid.

**File:** `WordSearch.Domain/Entities/Match.cs`

```csharp
public sealed class Match
{
    public string Word { get; }
    public GridCoordinate Start { get; }
    public GridCoordinate End { get; }

    public IEnumerable<GridCoordinate> GetPath();
    public static Match Parse(string matchString);
}
```

| Property | Type | Description |
|----------|------|-------------|
| `Word` | `string` | The word that was found |
| `Start` | `GridCoordinate` | Starting position in grid |
| `End` | `GridCoordinate` | Ending position in grid |

**Methods:**

| Method | Returns | Description |
|--------|---------|-------------|
| `GetPath()` | `IEnumerable<GridCoordinate>` | All cells the word occupies |
| `ToString()` | `string` | Format: `"WORD 0:0 0:3"` |
| `Parse(string)` | `Match` | Parse from string format |

---

## Value Objects

Value objects are immutable objects defined by their values, not identity.

### GridCoordinate

A position in the word search grid.

**File:** `WordSearch.Domain/ValueObjects/GridCoordinate.cs`

```csharp
public readonly record struct GridCoordinate(int Row, int Col)
{
    public static GridCoordinate Parse(string coordinate);
}
```

| Property | Type | Description |
|----------|------|-------------|
| `Row` | `int` | Zero-based row index |
| `Col` | `int` | Zero-based column index |

**String Format:** `"row:col"` (e.g., `"5:3"`)

**Why a record struct?**
- Immutable (thread-safe, no side effects)
- Value equality (`new GridCoordinate(1, 2) == new GridCoordinate(1, 2)`)
- Stack-allocated (no heap pressure)

---

### Dimensions

The size of a word search grid.

**File:** `WordSearch.Domain/ValueObjects/Dimensions.cs`

```csharp
public readonly record struct Dimensions(int Rows, int Columns)
{
    public static Dimensions Parse(string dimensions);
    public static bool TryParse(string dimensions, out Dimensions result);
}
```

| Property | Type | Description |
|----------|------|-------------|
| `Rows` | `int` | Number of rows |
| `Columns` | `int` | Number of columns |

**String Format:** `"ROWSxCOLUMNS"` (e.g., `"10x15"`)

---

## Enums

### DiagonalDirection

Direction for diagonal word searching.

**File:** `WordSearch.Domain/Enums/Direction.cs`

```csharp
public enum DiagonalDirection
{
    FromLeft,   // Top-left to bottom-right (\)
    FromRight   // Top-right to bottom-left (/)
}
```

| Value | Description |
|-------|-------------|
| `FromLeft` | Diagonal going ↘ |
| `FromRight` | Diagonal going ↙ |

---

### MatrixShape

Classification of grid shape.

**File:** `WordSearch.Domain/Enums/MatrixShape.cs`

```csharp
public enum MatrixShape
{
    Tall,    // More rows than columns
    Wide,    // More columns than rows
    Square   // Equal rows and columns
}
```

Used by the diagonal finder to optimize search patterns.

---

## Usage Example

```csharp
// Parse a puzzle from file content
var puzzle = WordSearchPuzzle.Parse(fileContent);

// Access the grid
char letter = puzzle.Letters[0, 0]; // First cell

// Check dimensions
Console.WriteLine($"Grid is {puzzle.Dimensions}"); // "10x10"

// Iterate words
foreach (var word in puzzle.Words)
{
    Console.WriteLine(word);
}
```
