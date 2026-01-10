# Session Notes - January 9, 2026

## Completed: CSS Architecture Overhaul

### What Was Done

1. **Removed Bootstrap entirely**
   - Deleted `/wwwroot/css/bootstrap/` folder
   - Removed Bootstrap reference from `index.html`

2. **Created new CSS architecture with four-tier hierarchy:**

   | File | Purpose | Size |
   |------|---------|------|
   | `foundation.css` | Design system primitives (colors, spacing, typography, shadows, z-index, transitions) + CSS reset | 14KB |
   | `its.css` | ITS semantic tokens (surfaces, buttons, forms, components) + base element styles | 30KB |
   | `brand.css` | Client branding overrides (empty placeholder for non-custom clients) | <1KB |
   | `app.css` | Word Search application-specific styles | 4KB |

3. **Updated `index.html` with new CSS loading order:**
   ```html
   <link rel="stylesheet" href="css/foundation.css" />
   <link rel="stylesheet" href="css/its.css" />
   <link rel="stylesheet" href="css/brand.css" />
   <link rel="stylesheet" href="css/app.css" />
   ```

4. **Kept new features** (Tooltips, User Guide link) - these were functional improvements not styling issues

### CSS Architecture Rules Established

1. **Hierarchy:** Foundation -> ITS -> Brand -> App -> Component
2. **Component CSS is sacrosanct** - never remove component styles to use higher-level styles
3. **Use semantic tokens** - components reference `--its-*` tokens, not `--f-*` primitives
4. **Brand overrides** - paying clients override `--its-*` tokens in `brand.css`

### Naming Convention

- `--f-*` = Foundation primitives (raw values)
- `--its-*` = ITS semantic tokens (purpose-driven)
- `.f-*` = Foundation utility classes (if needed)
- Component CSS = scoped, no prefix needed

### Key Commands

```bash
# Run the app
dotnet run --project WordSearch.Web

# Build
dotnet build WordSearch.Web
```

### Files Changed

- `/wwwroot/css/bootstrap/` - DELETED
- `/wwwroot/css/foundation.css` - CREATED
- `/wwwroot/css/its.css` - CREATED
- `/wwwroot/css/brand.css` - CREATED
- `/wwwroot/css/app.css` - REPLACED (original styles using semantic tokens)
- `/wwwroot/index.html` - UPDATED (new CSS loading order)

### Next Steps

1. Run the application and visually verify styling works
2. Test all interactive features (hover effects, buttons, tooltips)
3. Fix any styling issues that arise from Bootstrap removal
4. Consider migrating component CSS to use semantic tokens where appropriate
