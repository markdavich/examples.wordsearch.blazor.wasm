# Cloud Platforms

The app uses serverless functions to securely call AI APIs. These functions run on cloud platforms and hold the API keys.

## Why Two Platforms?

**Redundancy:** If one platform has issues, users can switch to the other.

**Flexibility:** Different platforms may have different rate limits or features.

**User Choice:** Users may prefer one provider over another.

## Cloudflare Workers

[Cloudflare Workers](https://workers.cloudflare.com/) is a serverless platform that runs code at the edge.

### Advantages

- **Fast:** Runs in 300+ data centers worldwide
- **Free tier:** 100,000 requests/day
- **Native AI:** Has built-in Workers AI models
- **Simple:** No cold starts, instant deployment

### Supported AI Providers

| Provider | Model | Notes |
|----------|-------|-------|
| Groq | Llama 3.2 Vision | Fast, free tier |
| Gemini | Gemini 1.5 Flash | Google's vision model |
| Together | Llama 3.2 Vision | Alternative to Groq |
| Cloudflare AI | LLaVA | Native, no external API |

### Worker Code Structure

```javascript
// Simplified worker structure
export default {
  async fetch(request, env) {
    const { image, provider } = await request.json();

    // Select AI provider
    const apiKey = env[`${provider.toUpperCase()}_API_KEY`];

    // Call AI API
    const result = await callAI(image, provider, apiKey);

    return new Response(JSON.stringify(result));
  }
}
```

### Environment Variables

```
GROQ_API_KEY=gsk_xxx
GEMINI_API_KEY=xxx
TOGETHER_API_KEY=xxx
```

---

## Vercel Functions

[Vercel](https://vercel.com/) is a platform for frontend frameworks with built-in serverless functions.

### Advantages

- **Easy deployment:** Git push to deploy
- **Free tier:** 100GB bandwidth/month
- **Familiar:** Uses standard Node.js
- **Logging:** Built-in function logs

### Supported AI Providers

| Provider | Model | Notes |
|----------|-------|-------|
| Groq | Llama 3.2 Vision | Fast, free tier |
| Gemini | Gemini 1.5 Flash | Google's vision model |
| Together | Llama 3.2 Vision | Alternative to Groq |

::: warning
Cloudflare Workers AI is not available on Vercel.
:::

### Function Code Structure

```typescript
// api/parse.ts
import { NextRequest, NextResponse } from 'next/server';

export async function POST(request: NextRequest) {
  const { image, provider } = await request.json();

  const apiKey = process.env[`${provider.toUpperCase()}_API_KEY`];

  const result = await callAI(image, provider, apiKey);

  return NextResponse.json(result);
}
```

### Environment Variables

Set in Vercel Dashboard → Settings → Environment Variables:

```
GROQ_API_KEY=gsk_xxx
GEMINI_API_KEY=xxx
TOGETHER_API_KEY=xxx
```

---

## Security

### API Key Protection

API keys are stored as environment variables in the cloud platform, never in client code.

```
Browser                 Serverless Function
   │                           │
   │  {image: base64}          │
   ├──────────────────────────▶│
   │                           │ API_KEY from env
   │                           │──────┐
   │                           │      │ Call AI API
   │                           │◀─────┘
   │  {puzzleText: ...}        │
   │◀──────────────────────────┤
```

### What the Browser Sees

```javascript
// Request
{
  image: "base64...",
  provider: "groq"
}

// Response
{
  success: true,
  puzzleData: "10x10\nABC..."
}
```

The browser never sees the API key.

### Rate Limiting

Both platforms support rate limiting to prevent abuse:

- **Cloudflare:** Use Workers Rate Limiting
- **Vercel:** Use Edge Middleware or Upstash

---

## Deployment

### Cloudflare Workers

```bash
# Install wrangler CLI
npm install -g wrangler

# Login
wrangler login

# Deploy
wrangler deploy

# Set secrets
wrangler secret put GROQ_API_KEY
```

### Vercel Functions

```bash
# Install Vercel CLI
npm install -g vercel

# Deploy
vercel

# Set environment variables in dashboard
# or use vercel env add
```

---

## Choosing a Platform

| Consideration | Cloudflare | Vercel |
|---------------|------------|--------|
| Speed | Faster (edge) | Good |
| Free tier | More generous | Good |
| Native AI | Yes (Workers AI) | No |
| Setup | Requires wrangler | Git push |
| Logging | Basic | Better |

**Recommendation:** Start with Cloudflare for speed and Workers AI access. Use Vercel as a fallback.
