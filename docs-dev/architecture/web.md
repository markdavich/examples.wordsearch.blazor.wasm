# Web Layer

The Web layer contains the Blazor WebAssembly application, including all UI components, pages, and styling.

## What Belongs Here

- **Pages**: Routable components (`Index.razor`, `About.razor`)
- **Components**: Reusable UI elements
- **Layouts**: Page structure (`NavBar`, `MainLayout`)
- **CSS**: Styling and theming
- **JavaScript Interop**: Browser API calls
- **Static Assets**: Images, fonts, icons

## What Does NOT Belong Here

- Business logic or algorithms (that's Domain)
- HTTP client implementations (that's Infrastructure)
- Interface definitions (that's Application)

## Project Structure

```
WordSearch.Web/
├── Components/          # Reusable UI components
│   ├── Tooltip.razor
│   ├── FileInput.razor
│   ├── AIUpload.razor
│   ├── ExampleSelector.razor
│   ├── UploadDropdown.razor
│   ├── WordSearchGrid.razor
│   └── [Component].razor.css
├── Layout/              # Page layouts
│   ├── MainLayout.razor
│   └── NavBar.razor
├── Pages/               # Routable pages
│   ├── Index.razor
│   └── About.razor
├── Services/            # Web-specific services
│   └── PuzzleService.cs
└── wwwroot/             # Static files
    ├── css/
    ├── images/
    └── index.html
```

## Key Components

### WordSearchGrid

Displays the puzzle grid with highlighting for found words.

```razor
@* WordSearchGrid.razor *@
<div class="grid-container" @ref="_containerRef">
    <div class="grid" style="@_scaleStyle">
        @for (int row = 0; row < Puzzle.Grid.Rows; row++)
        {
            <div class="row">
                @for (int col = 0; col < Puzzle.Grid.Columns; col++)
                {
                    <span class="@GetCellClass(row, col)">
                        @Puzzle.Grid[row, col]
                    </span>
                }
            </div>
        }
    </div>
</div>
```

**Features:**
- Auto-scales to fit container
- Highlights found words on hover
- Uses CSS Grid for layout

### Tooltip

A reusable tooltip component with smart positioning.

```razor
@* Usage *@
<Tooltip Title="Help"
         Description="Click to learn more"
         Position="top">
    <button>?</button>
</Tooltip>
```

**Features:**
- Fixed positioning (never clipped by containers)
- Auto-flips if would go off-screen
- Configurable position (top, bottom, left, right)

### FileInput

Handles file upload with drag-and-drop support.

```razor
<InputFile OnChange="HandleFileChange"
           accept=".wordsearch,.txt"
           class="file-input" />
```

### AIUpload

Captures images for AI parsing.

```razor
<AIUpload OnImageCaptured="HandleImage" />
```

**Features:**
- Camera capture or file upload
- Platform/provider selection
- Progress indication

## Component Communication

Components communicate through:

1. **Parameters** (parent to child)
2. **EventCallbacks** (child to parent)
3. **PuzzleService** (any to any)

### Event-Based Pattern

```csharp
// In ExampleSelector.razor
@inject PuzzleService PuzzleService

private async Task LoadExample(string filename)
{
    var content = await Http.GetStringAsync($"examples/{filename}");
    PuzzleService.LoadPuzzle(content);  // Fire event
}

// In Index.razor
protected override void OnInitialized()
{
    PuzzleService.OnPuzzleLoaded += HandlePuzzleLoaded;
}

private void HandlePuzzleLoaded(string content)
{
    _puzzle = Puzzle.Parse(content);
    _puzzle.Solve();
    StateHasChanged();
}
```

## CSS Architecture

The project uses CSS Custom Properties for theming:

```css
/* Foundation tokens (primitives) */
--f-neutral-900: #1A1A1A;
--f-space-4: 1rem;

/* Semantic tokens (purpose) */
--its-text-default: var(--f-neutral-900);
--its-surface-default: var(--f-neutral-0);
```

**Scoped CSS**: Each component has a `.razor.css` file that's automatically scoped to that component.

```css
/* Tooltip.razor.css */
.tooltip-popup {
    position: fixed;
    /* Only applies to Tooltip component */
}
```

## JavaScript Interop

For browser APIs not available in Blazor:

```csharp
// Get element dimensions
await JSRuntime.InvokeAsync<ElementSize>(
    "getElementSize",
    _containerRef);
```

```javascript
// In index.html
window.getElementSize = (element) => ({
    width: element.offsetWidth,
    height: element.offsetHeight
});
```

## Design Decisions

### Why Blazor WebAssembly?

- **C# Everywhere**: Share code between layers
- **No Server Required**: Runs entirely in browser
- **Type Safety**: Catch errors at compile time
- **Component Model**: Similar to React/Vue patterns

### Why Scoped CSS?

- **Encapsulation**: Styles don't leak between components
- **Maintainability**: CSS lives with its component
- **No Naming Conflicts**: Blazor adds unique identifiers

### Why PuzzleService for Events?

Navbar components can't have direct references to Index page. The service acts as an event bus, allowing loose coupling between unrelated components.
