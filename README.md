# Alphabet Soup - Word Search Solver

A word search puzzle solver built with Blazor WebAssembly following Clean Architecture principles.

## Features

- Upload word search puzzles from text files
- AI-powered image parsing (via Cloudflare Workers or Vercel)
- Automatic word finding in all directions (horizontal, vertical, diagonal)
- Visual highlighting of found words
- Circle overlay visualization

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (for building the user guide documentation)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/WordSearch.git
cd WordSearch
```

### Run the Application

```bash
cd WordSearch.Web
dotnet run
```

The application will start and be available at `https://localhost:5001` or `http://localhost:5000`.

### Build for Production

```bash
dotnet publish -c Release
```

The published output will be in `WordSearch.Web/bin/Release/net8.0/publish/wwwroot/`.

## Project Structure

```
WordSearch/
├── WordSearch.Domain/        # Core business entities
├── WordSearch.Application/   # Use cases and interfaces
├── WordSearch.Infrastructure/# External services
├── WordSearch.Web/           # Blazor WASM presentation layer
└── docs/                     # VitePress user guide source
```

## Architecture

This application follows Clean Architecture with four layers:

- **Domain:** Core business entities (WordSearch, Match, GridCoordinate)
- **Application:** Use cases and interfaces (IWordFinder, word finding services)
- **Infrastructure:** External services (API client, file conversion)
- **Web:** Blazor WebAssembly presentation layer

## Building the User Guide

The user guide is built with VitePress and outputs to the web application's wwwroot folder.

```bash
cd docs
npm install
npm run build
```

For development with hot reload:

```bash
npm run dev
```

## License

MIT
