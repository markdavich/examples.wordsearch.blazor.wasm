# AI Image Parsing

Alphabet Soup can extract word search puzzles from images using AI vision models.

## How It Works

1. Upload an image of a word search puzzle
2. Select your preferred AI provider and platform
3. The AI analyzes the image and extracts:
   - The letter grid
   - The word list
4. The extracted puzzle is loaded into the solver

## Supported Platforms

### Cloudflare Workers

A serverless platform that runs close to users worldwide. Select this for:
- Fast response times
- Global edge deployment

### Vercel

A popular serverless platform. Select this for:
- Alternative hosting option
- Different AI provider access

## AI Providers

### Groq

A fast inference provider that works with both platforms. Recommended for:
- Quick results
- Good accuracy

### Cloudflare AI

Available only with the Cloudflare Workers platform. Uses Cloudflare's built-in AI models.

## Tips for Best Results

### Image Quality

- Use clear, high-resolution images
- Ensure good lighting with minimal shadows
- Avoid blurry or skewed images

### Puzzle Format

- The letter grid should be clearly visible
- Word lists should be readable
- Standard word search layouts work best

### Troubleshooting

If the AI doesn't extract the puzzle correctly:
- Try a clearer image
- Crop the image to just the puzzle
- Use a different AI provider
- Manually create a text file as a fallback

## Privacy Note

Images are sent to the selected AI provider for processing. No images are stored permanently.
