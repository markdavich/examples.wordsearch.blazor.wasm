# VitePress Documentation

The project uses [VitePress](https://vitepress.dev/) for documentation. There are two separate documentation sites:

| Site | Folder | Purpose | Output |
|------|--------|---------|--------|
| User Guide | `docs/` | End-user documentation | `wwwroot/guide/` |
| Developer Guide | `docs-dev/` | Developer documentation | `wwwroot/dev-guide/` |

## Project Structure

```
docs/                          # User documentation
├── .vitepress/
│   ├── config.ts              # VitePress configuration
│   └── theme/
│       ├── index.ts           # Theme entry
│       └── custom.css         # ITS brand styling
├── index.md                   # Home page
├── getting-started.md
├── file-format.md
├── features.md
├── ai-parsing.md
└── package.json

docs-dev/                      # Developer documentation
├── .vitepress/
│   ├── config.ts
│   └── theme/
│       ├── index.ts
│       └── custom.css
├── index.md
├── architecture.md
├── architecture/
│   ├── domain.md
│   ├── application.md
│   ├── infrastructure.md
│   └── web.md
├── api/
│   ├── domain.md
│   ├── services.md
│   └── components.md
├── ai/
│   ├── overview.md
│   ├── platforms.md
│   └── services.md
├── getting-started.md
├── vitepress.md
└── package.json
```

## Configuration

### config.ts

```typescript
import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Alphabet Soup Developer Guide',
  description: 'Developer documentation',

  // Base URL path (important for routing)
  base: '/dev-guide/',

  // Output to Blazor's wwwroot folder
  outDir: '../WordSearch.Web/wwwroot/dev-guide',

  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Architecture', link: '/architecture' }
    ],
    sidebar: [
      {
        text: 'Guide',
        items: [
          { text: 'Introduction', link: '/' },
          { text: 'Getting Started', link: '/getting-started' }
        ]
      }
    ]
  }
})
```

### Key Options

| Option | Description |
|--------|-------------|
| `base` | URL path prefix (must match where it's served) |
| `outDir` | Build output directory |
| `title` | Site title in browser tab |
| `themeConfig.nav` | Top navigation links |
| `themeConfig.sidebar` | Side navigation structure |

## Theme Customization

The custom theme uses ITS brand colors:

```css
:root {
  --vp-c-brand-1: #00562A;  /* Primary green */
  --vp-c-brand-2: #2E8538;  /* Secondary green */
  --vp-c-brand-3: #3EB54B;  /* Accent green */
}
```

See `.vitepress/theme/custom.css` for full styling.

## Development

### Install Dependencies

```bash
cd docs-dev
npm install
```

### Start Dev Server

```bash
npm run dev
```

Opens at `http://localhost:5173/dev-guide/`

### Build for Production

```bash
npm run build
```

Outputs to `WordSearch.Web/wwwroot/dev-guide/`

## Writing Content

### Markdown Features

VitePress supports GitHub-flavored markdown plus extras:

```markdown
# Heading 1
## Heading 2

**bold** and *italic*

- List item
- Another item

1. Numbered
2. Items

`inline code`

[Link text](./other-page.md)
```

### Code Blocks

````markdown
```csharp
public class Example
{
    public void Method() { }
}
```
````

### Custom Containers

```markdown
::: tip
This is a tip
:::

::: warning
This is a warning
:::

::: danger
This is dangerous
:::

::: info
This is informational
:::
```

### Tables

```markdown
| Column 1 | Column 2 |
|----------|----------|
| Cell 1   | Cell 2   |
```

## Adding New Pages

1. Create a `.md` file in the appropriate folder
2. Add it to `config.ts` sidebar
3. Link to it from other pages

```typescript
// config.ts
sidebar: [
  {
    text: 'Guide',
    items: [
      { text: 'New Page', link: '/new-page' }  // Add here
    ]
  }
]
```

## Linking Between Docs

```markdown
<!-- Relative links (recommended) -->
[Architecture](./architecture.md)
[Domain Layer](./architecture/domain.md)

<!-- Absolute links -->
[Home](/)
```

## Integration with Blazor

### How It's Served

The built documentation is static HTML in `wwwroot/dev-guide/`. Kestrel serves these files directly.

### Linking from Blazor

```razor
<a href="dev-guide/index.html" target="_blank">Developers</a>
```

Use explicit `index.html` to bypass Blazor's SPA routing.

### Build Script

Consider adding a build script to `package.json`:

```json
{
  "scripts": {
    "build:docs": "cd docs && npm run build && cd ../docs-dev && npm run build"
  }
}
```

## Troubleshooting

### "Nothing at this address"

- Ensure link uses `/dev-guide/index.html` not just `/dev-guide/`
- Blazor's SPA routing intercepts directory URLs

### Styles not loading

- Check `base` in config matches the URL path
- Rebuild after config changes

### Changes not appearing

- Hard refresh (Ctrl+Shift+R) to clear cache
- Ensure you ran `npm run build`
