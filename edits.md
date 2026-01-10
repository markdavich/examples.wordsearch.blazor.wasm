# Edit Log

## Saturday, Jan 11, 2026, 01:01 AM

### Edit 1 - Fix Tooltips Being Cut Off by Container

**Problem:** Tooltips were being clipped by parent containers that have `overflow: hidden` set, making them partially or completely invisible.

**Solution:** Changed tooltip positioning from `position: absolute` (relative to parent) to `position: fixed` (relative to viewport).

**Changes Made:**

1. **Tooltip.razor** - Rewrote positioning logic:
   - Calculate tooltip position based on wrapper element's bounding rect in viewport coordinates
   - Automatically flip position if it would go off-screen (e.g., if `position="top"` but there's no room above, flip to bottom)
   - Clamp final position to stay within viewport bounds with 8px padding from edges
   - Apply position via inline styles (`top` and `left` in pixels)

2. **Tooltip.razor.css** - Simplified positioning CSS:
   - Changed `.tooltip-popup` from `position: absolute` to `position: fixed`
   - Removed complex flip override classes (`.tooltip-flip-left`, etc.) - now handled in C# logic
   - Position classes now only handle transform offsets and arrow placement

**Why This Works:** Fixed positioning removes the tooltip from the normal document flow and positions it relative to the viewport. This means parent containers with `overflow: hidden` cannot clip the tooltip, ensuring it's always 100% visible and never goes off-screen.

---

## Saturday, Jan 11, 2026, 01:05 AM

### Edit 2 - Create Alphabet Soup Logo and Favicon

**Request:** Create a nice logo for the app.

**Solution:** Designed an SVG logo that visually represents both "word search" and "alphabet soup" concepts.

**Design Elements:**
- **Soup bowl shape** - Curved bowl outline with open top
- **Steam wisps** - Three rising steam lines above the bowl (adds warmth/life)
- **Floating letters** - "WORD SOUP" letters arranged inside the bowl like alphabet soup
- **Highlight ellipse** - Tilted oval representing a "found word" circle (ties to app functionality)
- **Color scheme** - ITS green palette (#00562A, #3EB54B, #01391C)

**Files Created/Updated:**

1. **`wwwroot/images/logo.svg`** - Full standalone logo file for general use

2. **`wwwroot/favicon.svg`** - Simplified 32x32 version for browser tab
   - Smaller letters ("ABC", "XY") to remain legible at small sizes

3. **`Layout/NavBar.razor`** - Embedded inline SVG in navbar
   - Uses `currentColor` for bowl and letters so it inherits header text color
   - Wrapped in anchor link to homepage

4. **`Layout/NavBar.razor.css`** - Added logo link styles
   - Hover effect transitions color
   - 36x36px size in navbar

5. **`wwwroot/index.html`** - Updated favicon reference
   - Changed from PNG to SVG format

**Why SVG:** Scalable at any size, small file size, supports CSS color inheritance via `currentColor`, and works as both favicon and large logo.

---

## Saturday, Jan 11, 2026, 01:11 AM

### Edit 3 - Extract ExampleSelector Component

**Request:** Convert the example dropdown in FileInput.razor into a separate component.

**Solution:** Created a new `ExampleSelector` component that encapsulates the example puzzle selection functionality.

**Files Created:**

1. **`Components/ExampleSelector.razor`**
   - Self-contained component with Tooltip wrapper
   - Contains the static list of example files
   - Injects HttpClient for loading example content
   - Exposes three EventCallbacks:
     - `OnExampleLoaded` - fires with file content when loaded
     - `OnError` - fires with error message on failure
     - `OnFileNameChanged` - fires with filename for display

2. **`Components/ExampleSelector.razor.css`**
   - Moved `.example-select` styles from FileInput
   - Updated to use semantic CSS tokens (`--its-*` variables)
   - Added focus state styling

**Files Updated:**

1. **`Components/FileInput.razor`**
   - Removed inline example dropdown markup
   - Removed `ExampleFiles` list and `HandleExampleSelected` method
   - Removed `HttpClient` injection (no longer needed)
   - Added `<ExampleSelector>` with callback handlers

2. **`Components/FileInput.razor.css`**
   - Removed `.example-select` styles (now in component CSS)

**Benefits:**
- **Reusability** - ExampleSelector can be used independently elsewhere
- **Single Responsibility** - FileInput focuses on file input, ExampleSelector on examples
- **Maintainability** - Example list changes only affect one file
- **Encapsulation** - HTTP loading logic is contained within the component
