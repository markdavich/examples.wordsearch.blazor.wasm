namespace WordSearch.Web.Services;

public class PuzzleService
{
    public event Action<string>? OnPuzzleLoaded;
    public event Action<string>? OnPlatformChanged;
    public event Action<string>? OnAiProviderChanged;

    public string SelectedPlatform { get; private set; } = "cloudflare";
    public string SelectedAiProvider { get; private set; } = "groq";

    public void LoadPuzzle(string content)
    {
        OnPuzzleLoaded?.Invoke(content);
    }

    public void SetPlatform(string platform)
    {
        SelectedPlatform = platform;
        if (platform != "cloudflare" && SelectedAiProvider == "cloudflare-ai")
        {
            SelectedAiProvider = "groq";
            OnAiProviderChanged?.Invoke(SelectedAiProvider);
        }
        OnPlatformChanged?.Invoke(platform);
    }

    public void SetAiProvider(string provider)
    {
        SelectedAiProvider = provider;
        OnAiProviderChanged?.Invoke(provider);
    }
}
