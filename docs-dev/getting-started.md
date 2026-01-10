# Getting Started

This guide will help you set up the development environment, run the app, and understand the development workflow.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/) (for documentation)
- [Git](https://git-scm.com/)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/), or [Rider](https://www.jetbrains.com/rider/)

## Clone the Repository

```bash
git clone https://github.com/yourusername/WordSearchSolver.git
cd WordSearchSolver
```

## Project Structure

```
WordSearchSolver/
├── WordSearch.Domain/           # Core business logic
├── WordSearch.Application/      # Interfaces and services
├── WordSearch.Infrastructure/   # External API implementations
├── WordSearch.Web/              # Blazor WebAssembly app
├── WordSearch.Tests/            # Unit tests
├── docs/                        # User documentation
├── docs-dev/                    # Developer documentation
└── WordSearchSolver.sln         # Solution file
```

## Running the App

### Using .NET CLI

```bash
# From solution root
cd WordSearch.Web
dotnet run
```

Opens at `https://localhost:5001` or `http://localhost:5000`

### Using Visual Studio

1. Open `WordSearchSolver.sln`
2. Set `WordSearch.Web` as startup project
3. Press F5 or click "Run"

### Using VS Code

1. Open the folder in VS Code
2. Install C# extension
3. Press F5 (uses `.vscode/launch.json`)

## Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific project
dotnet test WordSearch.Tests
```

## Building Documentation

### User Documentation

```bash
cd docs
npm install
npm run dev      # Development server
npm run build    # Build to wwwroot/guide/
```

### Developer Documentation

```bash
cd docs-dev
npm install
npm run dev      # Development server
npm run build    # Build to wwwroot/dev-guide/
```

## Development Workflow

### 1. Create a Branch

```bash
git checkout -b feature/your-feature-name
```

### 2. Make Changes

Follow these guidelines:
- Domain logic goes in `WordSearch.Domain`
- Interfaces go in `WordSearch.Application`
- External API code goes in `WordSearch.Infrastructure`
- UI components go in `WordSearch.Web/Components`

### 3. Run Tests

```bash
dotnet test
```

### 4. Commit Changes

```bash
git add .
git commit -m "Add feature: description"
```

### 5. Push and Create PR

```bash
git push -u origin feature/your-feature-name
```

Then create a Pull Request on GitHub.

## Common Tasks

### Adding a New Component

1. Create `ComponentName.razor` in `WordSearch.Web/Components/`
2. Create `ComponentName.razor.css` for scoped styles
3. Use the component: `<ComponentName />`

### Adding a New Service

1. Define interface in `WordSearch.Application/Interfaces/`
2. Implement in appropriate layer
3. Register in `Program.cs`:

```csharp
builder.Services.AddSingleton<IMyService, MyService>();
```

### Modifying the Grid Algorithm

1. Edit finders in `WordSearch.Application/Services/`
2. Run tests to ensure correctness
3. Test with example puzzles in the UI

## Debugging

### Browser DevTools

1. Open app in browser
2. F12 to open DevTools
3. Check Console for errors
4. Network tab for API calls

### Blazor Debugging

In VS Code or Visual Studio:
1. Set breakpoints in `.razor` or `.cs` files
2. Run in Debug mode (F5)
3. Interact with the app to hit breakpoints

### Logging

```csharp
// In a component or service
Console.WriteLine($"Debug: {variable}");
```

Check browser console for output.

## Deployment

### Build for Production

```bash
dotnet publish WordSearch.Web -c Release -o ./publish
```

### Deploy to Static Host

The output is a static site. Deploy `publish/wwwroot/` to:
- GitHub Pages
- Netlify
- Vercel
- Azure Static Web Apps

### With Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish WordSearch.Web -c Release -o /publish

FROM nginx:alpine
COPY --from=build /publish/wwwroot /usr/share/nginx/html
```

## Troubleshooting

### "Port already in use"

```bash
# Find process using port
lsof -i :5000
# Or on Windows
netstat -ano | findstr :5000

# Kill the process or use different port
dotnet run --urls=http://localhost:5050
```

### NuGet restore fails

```bash
dotnet nuget locals all --clear
dotnet restore
```

### CSS not updating

1. Clear browser cache (Ctrl+Shift+R)
2. Check file is saved
3. Restart dev server

### Tests fail after changes

1. Run `dotnet build` to check for compile errors
2. Check test output for specific failures
3. Ensure test data matches expected format

## Getting Help

- Check existing [GitHub Issues](https://github.com/yourusername/WordSearchSolver/issues)
- Review this documentation
- Create a new issue with reproduction steps
