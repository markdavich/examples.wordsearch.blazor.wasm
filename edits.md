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

---

## Saturday, Jan 11, 2026, 01:21 AM

### Edit 4 - Move Upload Controls to Navbar

**Request:** Put FileInput and AIUpload in a dropdown titled "Upload" in the navbar, then move ExampleSelector to the navbar as well.

**Solution:** Created an event-based architecture to allow navbar components to communicate with the Index page, then moved all upload controls to the navbar.

**Architecture:**

Created `PuzzleService` as a singleton service that acts as an event bus:
- `OnPuzzleLoaded` event - fired when any puzzle is loaded
- `SetPlatform()` / `SetAiProvider()` - manage AI settings
- Registered in `Program.cs` as singleton for app-wide sharing

**Files Created:**

1. **`Services/PuzzleService.cs`**
   - Simple event service for cross-component communication
   - Stores platform/provider preferences
   - Fires `OnPuzzleLoaded` when puzzles are loaded

2. **`Components/UploadDropdown.razor`**
   - Dropdown trigger button with upload icon
   - Contains FileInput and AIUpload in dropdown menu
   - Closes dropdown after successful puzzle load

3. **`Components/UploadDropdown.razor.css`**
   - Fixed positioning dropdown menu
   - Styled trigger to match navbar links
   - Animated dropdown arrow

**Files Updated:**

1. **`Layout/NavBar.razor`**
   - Added `<UploadDropdown />` to navbar-links
   - Added `<ExampleSelector />` to navbar-links

2. **`Components/ExampleSelector.razor`**
   - Now uses PuzzleService instead of EventCallbacks
   - Simplified to just load and notify

3. **`Components/ExampleSelector.razor.css`**
   - Restyled for navbar (transparent, matching colors)
   - Custom SVG dropdown arrow

4. **`Components/FileInput.razor`**
   - Removed ExampleSelector (now in navbar separately)
   - Simplified to just file input functionality

5. **`Pages/Index.razor`**
   - Removed FileInput and AIUpload from page
   - Subscribes to `PuzzleService.OnPuzzleLoaded`
   - Implements `IDisposable` for cleanup

6. **`Program.cs`**
   - Registered `PuzzleService` as singleton

**Benefits:**
- **Cleaner page layout** - Controls moved to navbar, more space for puzzle
- **Decoupled architecture** - Components communicate via events
- **Consistent UX** - All upload options accessible from navbar
- **Reusable service** - PuzzleService can be used by future pages

---

## Saturday, Jan 11, 2026, 01:37 AM

### Edit 5 - Fix FileInput Styling to Match AIUpload

**Problem:** FileInput text was white on white background, making it invisible. The component styling didn't match AIUpload.

**Root Cause:** Missing CSS variables in its.css:
- `--its-surface-default` (white background)
- `--its-text-default` (dark text)
- `--its-text-subtle` (muted text)

**Solution:** Added missing CSS variables and updated FileInput styling.

**Files Updated:**

1. **`wwwroot/css/its.css`** - Added missing semantic tokens:
   - `--its-surface-default: var(--f-neutral-0)` - white surface
   - `--its-text-default: var(--f-neutral-900)` - dark text
   - `--its-text-subtle: var(--f-neutral-600)` - muted text

2. **`Components/FileInput.razor`**
   - Changed label to h4 with tooltip (matching AIUpload structure)

3. **`Components/FileInput.razor.css`**
   - Added gray container background (`--its-surface-sunken`)
   - Added padding and border-radius
   - Added explicit `color: var(--its-text-default)` to file input
   - Styled `::file-selector-button` pseudo-element with proper colors
   - Added h4 styling

**Result:** FileInput now has visible text and matches AIUpload's visual style with gray container and white input area.

---

## Saturday, Jan 11, 2026, 01:42 AM

### Edit 6 - Fix User Guide Link

**Problem:** Clicking "User Guide" in the navbar opened `/guide/` but showed Blazor's "Sorry, there's nothing at this address" instead of the static VitePress documentation.

**Root Cause:** Blazor WebAssembly's SPA router intercepts all URL paths. When requesting `/guide/`, the DevServer falls back to serving the root `index.html`, and Blazor's router shows its NotFound component because there's no `/guide` page route.

**Solution:** Changed the href from `guide/` to `guide/index.html`.

**File Updated:**
- **`Layout/NavBar.razor`** - Changed User Guide link to explicit file path

**Why This Works:** When requesting a specific file path (`guide/index.html`), the DevServer recognizes it as an existing static file and serves it directly, bypassing Blazor's SPA fallback behavior.

---

## Friday, Jan 10, 2026, 02:10 AM

### Edit 7 - Create Developer Documentation

**Request:** Add comprehensive developer documentation accessible via "Developers" link in navbar.

**Solution:** Created a new VitePress site in `docs-dev/` for developer documentation.

**Files Created:**

1. **`docs-dev/package.json`** - VitePress project configuration
2. **`docs-dev/.vitepress/config.ts`** - Site configuration with sidebar
3. **`docs-dev/.vitepress/theme/index.ts`** - Theme entry
4. **`docs-dev/.vitepress/theme/custom.css`** - ITS brand styling

**Documentation Files:**

- **`index.md`** - Home page with overview and quick links
- **`architecture.md`** - Architecture overview
- **`architecture/domain.md`** - Domain layer documentation
- **`architecture/application.md`** - Application layer documentation
- **`architecture/infrastructure.md`** - Infrastructure layer documentation
- **`architecture/web.md`** - Web layer documentation
- **`api/domain.md`** - Domain types reference (WordSearchPuzzle, Match, GridCoordinate, etc.)
- **`api/services.md`** - Services reference (WordFinderService, PuzzleService, etc.)
- **`api/components.md`** - Blazor components reference
- **`ai/overview.md`** - AI integration overview
- **`ai/platforms.md`** - Cloud platform documentation (Cloudflare, Vercel)
- **`ai/services.md`** - AI provider documentation (Groq, Gemini, Together, Cloudflare AI)
- **`getting-started.md`** - Developer getting started guide
- **`vitepress.md`** - VitePress site documentation

**Files Updated:**

1. **`Layout/NavBar.razor`** - Added "Developers" link pointing to `dev-guide/index.html`

**Output:** Documentation builds to `WordSearch.Web/wwwroot/dev-guide/`

**Documentation Covers:**
1. Clean Architecture explanation (all 4 layers with what belongs where)
2. All classes, interfaces, enums, structs with code examples
3. AI uploading technology, platforms, and providers
4. VitePress site setup and customization
5. Getting started guide (clone, debug, test, deploy)

---

## Friday, Jan 10, 2026, 02:47 AM

### Edit 8 - Improve Test Coverage to 80%+

**Request:** Add tests to achieve 80% test coverage (team standard).

**Previous State:** 59.5% line coverage with 338 tests

**Final State:** 81% line coverage with 416 tests

**Coverage by Assembly:**
| Assembly | Before | After |
|----------|--------|-------|
| WordSearch.Application | 96.7% | 100% |
| WordSearch.Domain | 100% | 100% |
| WordSearch.Infrastructure | 58.4% | 100% |
| WordSearch.Web | 40.2% | 67.3% |
| **Overall** | **59.5%** | **81%** |

**Test Files Created:**

1. **`WordSearch.Tests/Infrastructure/Services/PuzzleParserApiClientTests.cs`**
   - 14 tests for HTTP client with mocked HttpMessageHandler
   - Tests: success, 400/500 errors, network errors, timeouts, invalid JSON, empty responses
   - Tests platform URL selection (Cloudflare vs Vercel)
   - Tests IsAvailableAsync health checks

2. **`WordSearch.Tests.Ui/Pages/IndexTests.cs`**
   - 21 tests for the main Index page
   - Tests: initial render, puzzle loading, word finder integration
   - Tests: button interactions (Clear Highlights, Circle Answers)
   - Tests: word selection, disposal/unsubscription

**Test Files Expanded:**

1. **AIUploadTests.cs** - Expanded from 20 to 28 tests
   - Platform/provider parameter synchronization
   - UI element rendering verification

2. **UploadDropdownTests.cs** - Expanded from 12 to 24 tests
   - Menu open/close on file select and puzzle parse
   - Platform/provider change propagation to PuzzleService
   - Focus out delay behavior

3. **WordSearchGridTests.cs** - Expanded from 18 to 35 tests
   - Circle hover enter/leave events
   - Cell elevation on hover
   - Path stroke width and fill opacity changes
   - SVG dimensions based on grid size

4. **AnswersTests.cs** - Expanded from 14 to 26 tests
   - ScrollToWord functionality with JS interop verification
   - Multiple highlighted answers
   - Long word lists
   - Special characters in words

**Key Testing Patterns Used:**
- bUnit `TestContext` for Blazor component testing
- `Mock<HttpMessageHandler>` with `Protected()` for HTTP mocking
- `cut.InvokeAsync()` for dispatcher-safe event invocation
- `WaitForState()` for timer-based behavior testing
- Type aliases to resolve ambiguous references (`using DomainMatch = ...`)
