# Blazor Components

All UI components are in the `WordSearch.Web/Components` folder with scoped CSS files.

## Grid Components

### WordSearchGrid

Displays the puzzle grid with word highlighting.

**File:** `WordSearch.Web/Components/WordSearchGrid.razor`

```razor
<WordSearchGrid Puzzle="@_puzzle"
                HighlightedMatch="@_selectedMatch"
                OnCellClick="HandleCellClick" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `Puzzle` | `WordSearchPuzzle` | The puzzle to display |
| `HighlightedMatch` | `Match?` | Optional match to highlight |
| `OnCellClick` | `EventCallback<GridCoordinate>` | Cell click handler |

**Features:**
- Auto-scales to fit container using JavaScript interop
- Highlights cells of the selected match
- Uses CSS Grid for responsive layout

---

### WordsToFind

Displays the list of words to find with status.

**File:** `WordSearch.Web/Components/WordsToFind.razor`

```razor
<WordsToFind Words="@_puzzle.Words"
             FoundWords="@_foundWords"
             OnWordClick="HandleWordClick" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `Words` | `IReadOnlyList<string>` | All words to find |
| `FoundWords` | `IEnumerable<Match>` | Words already found |
| `OnWordClick` | `EventCallback<string>` | Word click handler |

---

### Answers

Displays found words with their coordinates.

**File:** `WordSearch.Web/Components/Answers.razor`

```razor
<Answers Matches="@_matches"
         OnMatchClick="HandleMatchClick" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `Matches` | `IEnumerable<Match>` | Found word matches |
| `OnMatchClick` | `EventCallback<Match>` | Match click handler |

---

## Input Components

### FileInput

Handles file upload for puzzle files.

**File:** `WordSearch.Web/Components/FileInput.razor`

```razor
<FileInput OnFileSelected="HandleFile"
           Label="Upload Puzzle" />
```

**Parameters:**

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `OnFileSelected` | `EventCallback<string>` | - | Fires with file content |
| `Label` | `string` | `"Select File"` | Label text |

**Accepts:** `.wordsearch`, `.txt` files

---

### AIUpload

Captures images for AI parsing.

**File:** `WordSearch.Web/Components/AIUpload.razor`

```razor
<AIUpload OnPuzzleParsed="HandleParsedPuzzle"
          OnError="HandleError" />
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnPuzzleParsed` | `EventCallback<string>` | Fires with parsed puzzle data |
| `OnError` | `EventCallback<string>` | Fires with error message |

**Features:**
- Camera capture or file upload
- Platform/provider selection dropdowns
- Progress indicator during parsing

---

### ExampleSelector

Dropdown to load example puzzles.

**File:** `WordSearch.Web/Components/ExampleSelector.razor`

```razor
<ExampleSelector />
```

This component has no parameters. It uses `PuzzleService` internally to fire the `OnPuzzleLoaded` event.

**Example Files:**
- `puzzle1.wordsearch`
- `puzzle2.wordsearch`
- (Located in `wwwroot/examples/`)

---

## Layout Components

### UploadDropdown

Dropdown containing FileInput and AIUpload.

**File:** `WordSearch.Web/Components/UploadDropdown.razor`

```razor
<UploadDropdown />
```

**Features:**
- Animated dropdown arrow
- Closes after successful puzzle load
- Fixed positioning for dropdown menu

---

### Tooltip

Smart tooltip with auto-positioning.

**File:** `WordSearch.Web/Components/Tooltip.razor`

```razor
<Tooltip Title="Help Title"
         Description="Detailed description text"
         Position="top"
         GuideLink="guide/page.html">
    <button>Hover me</button>
</Tooltip>
```

**Parameters:**

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Title` | `string` | - | Tooltip header |
| `Description` | `string` | - | Tooltip body |
| `Position` | `string` | `"top"` | `top`, `bottom`, `left`, `right` |
| `GuideLink` | `string?` | - | Optional link to docs |
| `ChildContent` | `RenderFragment` | - | Element to attach tooltip to |

**Features:**
- Fixed positioning (never clipped by containers)
- Auto-flips if would go off-screen
- Link to documentation if `GuideLink` provided

---

## Component Communication Patterns

### Parent-Child (Parameters & EventCallbacks)

```razor
@* Parent component *@
<ChildComponent Data="@_data" OnAction="HandleAction" />

@code {
    private void HandleAction(string result)
    {
        // Handle child's action
    }
}
```

### Sibling (via PuzzleService)

```razor
@* Component A - fires event *@
@inject PuzzleService PuzzleService

private void DoAction()
{
    PuzzleService.LoadPuzzle(content);
}

@* Component B - listens to event *@
@inject PuzzleService PuzzleService
@implements IDisposable

protected override void OnInitialized()
{
    PuzzleService.OnPuzzleLoaded += HandlePuzzle;
}

public void Dispose()
{
    PuzzleService.OnPuzzleLoaded -= HandlePuzzle;
}
```

---

## Scoped CSS

Each component has a `.razor.css` file:

```
Components/
├── Tooltip.razor
├── Tooltip.razor.css      ← Scoped to Tooltip only
├── FileInput.razor
├── FileInput.razor.css    ← Scoped to FileInput only
```

**How Blazor Scoped CSS Works:**

1. Blazor adds a unique attribute (e.g., `b-abc123`) to component elements
2. CSS selectors are rewritten to include this attribute
3. Styles only apply to their component

```css
/* What you write */
.tooltip-popup { ... }

/* What Blazor generates */
.tooltip-popup[b-abc123] { ... }
```
