# Domain Layer

The Domain layer is the heart of the application. It contains the core business logic that would exist even without a UI or database.

## What Belongs Here

- **Entities**: Objects with identity (e.g., `Puzzle`, `WordSearchGrid`)
- **Value Objects**: Immutable objects defined by their values (e.g., `Position`, `Direction`)
- **Enums**: Type-safe constants (e.g., `Direction`)
- **Business Rules**: The algorithm for solving word searches
- **Domain Exceptions**: Errors specific to business logic

## What Does NOT Belong Here

- HTTP clients or API calls
- UI code or Blazor components
- File system operations
- Configuration or settings
- Dependency injection setup

## Key Types

### Puzzle

The main entity representing a word search puzzle.

```csharp
public class Puzzle
{
    public WordSearchGrid Grid { get; }
    public IReadOnlyList<string> WordsToFind { get; }
    public IReadOnlyList<FoundWord> FoundWords { get; }

    public void Solve();
}
```

**Responsibilities:**
- Holds the grid and word list
- Contains the solving algorithm
- Tracks which words have been found

### WordSearchGrid

Represents the letter grid as a 2D array.

```csharp
public class WordSearchGrid
{
    public int Rows { get; }
    public int Columns { get; }
    public char this[int row, int col] { get; }

    public char? GetLetter(Position position);
}
```

**Responsibilities:**
- Provides safe access to grid letters
- Bounds checking for positions

### Position

A value object representing a location in the grid.

```csharp
public readonly record struct Position(int Row, int Column)
{
    public Position Move(Direction direction);
    public bool IsValid(int maxRow, int maxCol);
}
```

**Why a record struct?**
- Immutable (safer)
- Value equality (two positions with same row/col are equal)
- Lightweight (no heap allocation)

### Direction

An enum defining the 8 possible search directions.

```csharp
public enum Direction
{
    Right,      // →
    Left,       // ←
    Down,       // ↓
    Up,         // ↑
    DownRight,  // ↘
    DownLeft,   // ↙
    UpRight,    // ↗
    UpLeft      // ↖
}
```

### FoundWord

Represents a word that has been located in the grid.

```csharp
public class FoundWord
{
    public string Word { get; }
    public Position Start { get; }
    public Position End { get; }
    public Direction Direction { get; }
    public IReadOnlyList<Position> Positions { get; }
}
```

## The Solving Algorithm

The puzzle solver uses a brute-force search:

1. For each word to find:
2. For each position in the grid:
3. For each of the 8 directions:
4. Check if the word exists starting at that position going in that direction

```csharp
// Simplified algorithm
foreach (var word in WordsToFind)
{
    for (int row = 0; row < Grid.Rows; row++)
    {
        for (int col = 0; col < Grid.Columns; col++)
        {
            foreach (var direction in AllDirections)
            {
                if (TryFindWord(word, new Position(row, col), direction))
                {
                    // Word found!
                }
            }
        }
    }
}
```

## Design Decisions

### Why no interfaces in Domain?

The Domain layer defines behavior, not abstractions. Interfaces for external services belong in the Application layer. The Domain doesn't need to know HOW puzzles are loaded—only what a puzzle IS.

### Why immutable where possible?

Immutability prevents bugs from unexpected state changes. A `Position` can't change after creation, so you can safely pass it around without copying.

### Why is Solve() on Puzzle?

The solving algorithm is core business logic. It doesn't depend on any external services, so it belongs in the Domain. If we later add different solving strategies, we can introduce a `ISolver` interface in the Application layer.
