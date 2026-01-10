---
layout: home

hero:
  name: "Alphabet Soup"
  text: "Developer Documentation"
  tagline: Technical guide for developers contributing to the Word Search Solver
  actions:
    - theme: brand
      text: Getting Started
      link: /getting-started
    - theme: alt
      text: Architecture
      link: /architecture

features:
  - title: Clean Architecture
    details: Four-layer architecture separating concerns between Domain, Application, Infrastructure, and Web layers.
  - title: Blazor WebAssembly
    details: Modern SPA framework running entirely in the browser with C# and .NET.
  - title: AI-Powered Parsing
    details: Cloud-based image-to-text extraction using serverless functions and AI vision models.
  - title: Component-Based UI
    details: Reusable Blazor components with scoped CSS and event-driven communication.
---

# Welcome

This documentation is for developers who want to understand, modify, or contribute to the Alphabet Soup Word Search Solver.

## Quick Links

- [Architecture Overview](./architecture) - Understand the project structure
- [Getting Started](./getting-started) - Set up your development environment
- [API Reference](./api/domain) - Browse all classes and interfaces
- [AI Integration](./ai/overview) - Learn about the AI image parsing system

## Project Structure

```
WordSearchSolver/
├── WordSearch.Domain/        # Core business logic
├── WordSearch.Application/   # Use cases and interfaces
├── WordSearch.Infrastructure/# External implementations
├── WordSearch.Web/           # Blazor WebAssembly UI
├── WordSearch.Tests/         # Unit and integration tests
├── docs/                     # User documentation (VitePress)
└── docs-dev/                 # Developer documentation (this site)
```

## Technology Stack

| Layer | Technology |
|-------|------------|
| Frontend | Blazor WebAssembly |
| Language | C# 12, .NET 8 |
| Styling | CSS Custom Properties |
| Documentation | VitePress |
| AI Platforms | Cloudflare Workers, Vercel Functions |
| AI Services | Groq, Google Gemini, OpenAI |
