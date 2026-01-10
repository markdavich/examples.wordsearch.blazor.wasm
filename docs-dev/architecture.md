# Architecture Overview

Alphabet Soup follows **Clean Architecture** principles, organizing code into four distinct layers. Each layer has a specific purpose and set of rules about what it can and cannot do.

## The Four Layers

```
┌─────────────────────────────────────────────────────┐
│                    Web Layer                        │
│         (Blazor components, pages, UI)              │
├─────────────────────────────────────────────────────┤
│              Infrastructure Layer                   │
│    (HTTP clients, file I/O, external services)      │
├─────────────────────────────────────────────────────┤
│               Application Layer                     │
│      (Use cases, interfaces, orchestration)         │
├─────────────────────────────────────────────────────┤
│                 Domain Layer                        │
│    (Entities, value objects, business rules)        │
└─────────────────────────────────────────────────────┘
```

## Dependency Rule

**Dependencies only point inward.** This means:
- Domain knows nothing about Application, Infrastructure, or Web
- Application knows about Domain, but not Infrastructure or Web
- Infrastructure knows about Application and Domain
- Web knows about everything

This rule keeps the core business logic (Domain) independent of frameworks and external services.

## Layer Summary

| Layer | Project | Purpose |
|-------|---------|---------|
| Domain | `WordSearch.Domain` | Core business objects and algorithms |
| Application | `WordSearch.Application` | Interfaces and simple services |
| Infrastructure | `WordSearch.Infrastructure` | External API implementations |
| Web | `WordSearch.Web` | Blazor UI and user interaction |

## When to Add Code

**Ask yourself:** "What is this code responsible for?"

| If the code... | Put it in... |
|----------------|--------------|
| Defines what a puzzle IS | Domain |
| Defines the algorithm to SOLVE a puzzle | Domain |
| Defines HOW to get a puzzle from an external source | Infrastructure |
| Defines the UI for displaying a puzzle | Web |
| Orchestrates multiple operations | Application |

## Learn More

- [Domain Layer](./architecture/domain) - Entities, algorithms, and business rules
- [Application Layer](./architecture/application) - Interfaces and services
- [Infrastructure Layer](./architecture/infrastructure) - External API implementations
- [Web Layer](./architecture/web) - Blazor components and pages
