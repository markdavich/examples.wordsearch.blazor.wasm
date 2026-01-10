# AI Integration Overview

Alphabet Soup uses AI to extract word search puzzles from images. Users can take a photo of a printed puzzle and have it converted to text format automatically.

## Why AI?

Word search puzzles often come as:
- Printed worksheets
- Magazine pages
- PDFs
- Screenshots

Manually typing in a 15x15 grid is tedious and error-prone. AI vision models can read the image and extract the letter grid and word list in seconds.

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Browser                                  │
│  ┌─────────────┐    ┌──────────────┐    ┌──────────────────┐   │
│  │  AIUpload   │───▶│ IPuzzleParser│───▶│ PuzzleService    │   │
│  │  Component  │    │     Api      │    │ (fires event)    │   │
│  └─────────────┘    └──────────────┘    └──────────────────┘   │
│         │                                                        │
└─────────│────────────────────────────────────────────────────────┘
          │ HTTPS
          ▼
┌─────────────────────────────────────────────────────────────────┐
│              Serverless Functions (Cloud Platform)               │
│  ┌──────────────────┐         ┌─────────────────────────────┐  │
│  │ Cloudflare Worker│   OR    │     Vercel Function         │  │
│  │  (Holds API keys)│         │    (Holds API keys)         │  │
│  └────────┬─────────┘         └──────────────┬──────────────┘  │
└───────────│──────────────────────────────────│──────────────────┘
            │                                   │
            ▼                                   ▼
┌───────────────────────────────────────────────────────────────────┐
│                        AI Vision APIs                              │
│   ┌─────────┐   ┌─────────┐   ┌──────────┐   ┌──────────────┐   │
│   │  Groq   │   │ Gemini  │   │ Together │   │ Cloudflare   │   │
│   │ (Llama) │   │(Google) │   │   AI     │   │   Workers AI │   │
│   └─────────┘   └─────────┘   └──────────┘   └──────────────┘   │
└───────────────────────────────────────────────────────────────────┘
```

## How It Works

1. **User uploads image** via AIUpload component
2. **Browser sends image** to serverless function as base64
3. **Serverless function** adds API key and calls AI provider
4. **AI provider** analyzes image and returns structured text
5. **Response flows back** to browser
6. **Component parses response** into puzzle format

## Why Serverless Functions?

**Security:** API keys must be kept secret. If we called AI APIs directly from the browser, the keys would be visible in network requests.

**CORS:** Most AI APIs don't allow browser requests. Serverless functions act as a proxy.

**Rate Limiting:** Server-side code can implement rate limiting to prevent abuse.

## Data Flow

```
Image (bytes)
    → Base64 encode
    → JSON request
    → Serverless function
    → AI API
    → Structured text response
    → Parse to puzzle format
    → Display in UI
```

## Configuration

The user selects:

1. **Platform** - Which serverless provider to use
   - Cloudflare Workers
   - Vercel Functions

2. **AI Provider** - Which AI model to use
   - Groq (Llama 3.2 Vision)
   - Google Gemini
   - Together AI
   - Cloudflare Workers AI (Cloudflare only)

## Error Handling

Common failure scenarios:

| Error | Cause | User Action |
|-------|-------|-------------|
| "Invalid image" | Unsupported format | Use PNG, JPG, or WebP |
| "Rate limited" | Too many requests | Wait and try again |
| "Parse failed" | AI couldn't read image | Try different provider |
| "Network error" | Connection issue | Check internet |

## Learn More

- [Platforms](./platforms) - Detailed platform documentation
- [AI Services](./services) - AI provider comparison
