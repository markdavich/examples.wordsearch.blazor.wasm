using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WordSearchSolver.Application.Interfaces;
using WordSearchSolver.Application.Services;
using WordSearchSolver.Infrastructure.Configuration;
using WordSearchSolver.Infrastructure.Services;
using WordSearchSolver.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure API settings
builder.Services.Configure<ApiSettings>(options =>
{
    options.CloudflareUrl = "https://puzzle-parser-api.markdavich.workers.dev";
    options.VercelUrl = "https://examples-wordsearch.vercel.app/api/puzzle-parser";
});

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Application services
builder.Services.AddScoped<IWordFinder, WordFinderService>();

// Register Infrastructure services
builder.Services.AddScoped<IFileConverter, FileConverterService>();
builder.Services.AddScoped<IPuzzleParserApi, PuzzleParserApiClient>();

await builder.Build().RunAsync();
