# AI Services

The app supports multiple AI vision providers for image parsing. Each has different strengths.

## Provider Comparison

| Provider | Model | Speed | Quality | Free Tier | Notes |
|----------|-------|-------|---------|-----------|-------|
| Groq | Llama 3.2 90B Vision | Fast | Good | Yes | Recommended |
| Gemini | Gemini 1.5 Flash | Medium | Very Good | Yes | Google's model |
| Together | Llama 3.2 Vision | Medium | Good | Limited | Alternative |
| Cloudflare AI | LLaVA | Fast | Moderate | Yes | Cloudflare only |

## Groq

[Groq](https://groq.com/) provides extremely fast inference using custom LPU hardware.

### Model

**Llama 3.2 90B Vision** - Meta's open-source vision-language model.

### Strengths

- **Speed:** Fastest inference times (often <1 second)
- **Free tier:** Generous daily quota
- **Quality:** Good accuracy for structured text extraction

### Limitations

- Rate limits on free tier
- May struggle with very low quality images

### API Call

```javascript
const response = await fetch('https://api.groq.com/openai/v1/chat/completions', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${GROQ_API_KEY}`,
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    model: 'llama-3.2-90b-vision-preview',
    messages: [{
      role: 'user',
      content: [
        { type: 'text', text: PROMPT },
        { type: 'image_url', image_url: { url: `data:image/png;base64,${image}` } }
      ]
    }]
  })
});
```

---

## Google Gemini

[Gemini](https://ai.google.dev/) is Google's multimodal AI family.

### Model

**Gemini 1.5 Flash** - Fast, efficient model optimized for high-volume tasks.

### Strengths

- **Quality:** Excellent text recognition
- **Context:** Large context window
- **Reliability:** Backed by Google infrastructure

### Limitations

- Slightly slower than Groq
- Rate limits apply

### API Call

```javascript
const response = await fetch(
  `https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key=${GEMINI_API_KEY}`,
  {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      contents: [{
        parts: [
          { text: PROMPT },
          { inline_data: { mime_type: 'image/png', data: image } }
        ]
      }]
    })
  }
);
```

---

## Together AI

[Together AI](https://together.ai/) provides access to open-source models.

### Model

**Llama 3.2 Vision** - Same as Groq but on Together's infrastructure.

### Strengths

- Open-source models
- Good alternative if Groq is rate-limited

### Limitations

- Less generous free tier
- Slower than Groq

---

## Cloudflare Workers AI

[Workers AI](https://developers.cloudflare.com/workers-ai/) provides AI models built into Cloudflare.

### Model

**LLaVA** - Open-source vision-language model.

### Strengths

- **No external API:** Runs on Cloudflare's infrastructure
- **No API key needed:** Just enable Workers AI
- **Fast:** Edge deployment

### Limitations

- Only available on Cloudflare platform
- Lower quality than larger models
- Best for simple puzzles

### API Call

```javascript
// Inside Cloudflare Worker
const response = await env.AI.run('@cf/llava-hf/llava-1.5-7b-hf', {
  image: imageArrayBuffer,
  prompt: PROMPT
});
```

---

## The Prompt

All providers use the same system prompt to ensure consistent output:

```
You are a word search puzzle extractor. Given an image of a word search puzzle:

1. Extract the grid dimensions (e.g., "10x10")
2. Extract all letters in the grid, row by row
3. Extract all words to find

Output format:
- First line: dimensions (ROWSxCOLUMNS)
- Next lines: letter grid (one row per line, letters separated by spaces)
- Remaining lines: words to find (one per line)

Example output:
5x5
A B C D E
F G H I J
K L M N O
P Q R S T
U V W X Y
APPLE
BANANA
CAT
```

---

## Choosing a Provider

### For Best Speed
Use **Groq** - consistently fastest inference.

### For Best Quality
Use **Gemini** - best text recognition accuracy.

### For Cloudflare Users
Try **Cloudflare AI** first - no external API needed.

### If Rate Limited
Switch to **Together AI** or try a different provider.

---

## Getting API Keys

### Groq
1. Go to [console.groq.com](https://console.groq.com)
2. Sign up / Log in
3. Navigate to API Keys
4. Create new key

### Gemini
1. Go to [aistudio.google.com](https://aistudio.google.com)
2. Sign up / Log in
3. Get API key

### Together AI
1. Go to [together.ai](https://together.ai)
2. Sign up / Log in
3. Navigate to Settings → API Keys
4. Create new key

### Cloudflare AI
1. Enable Workers AI in Cloudflare Dashboard
2. No separate API key needed
3. Use `env.AI` in worker code
