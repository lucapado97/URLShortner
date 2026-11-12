using URLShortner;

var builder = WebApplication.CreateBuilder(args);

// Register UrlStore as a singleton (thread-safe, shared across all requests)
builder.Services.AddSingleton<UrlStore>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "URL Shortener API", Version = "v1" });
});

var app = builder.Build();

// Initialize the store with samples
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<UrlStore>();
    store.InitializeSampleUrls();
}

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API v1");
    c.RoutePrefix = "swagger";
});

// PUT /url - Add a URL and get a 6-digit short key
app.MapPut("/url", (HttpContext httpContext, string url, UrlStore store) =>
{
    if (string.IsNullOrWhiteSpace(url))
    {
        return Results.BadRequest("URL cannot be empty");
    }

    if (!Uri.TryCreate(url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL format. Please provide a valid absolute URL (e.g., https://example.com)");
    }

    string key = store.AddUrl(url);

    var request = httpContext.Request;
    string shortenedUrl = $"{request.Scheme}://{request.Host}/url/{key}";

    return Results.Ok(new { key = key, originalUrl = url, shortenedUrl = shortenedUrl });
})
.WithName("AddUrl");

// GET /url/{key} - Redirect to the original URL
app.MapGet("/url/{key}", (string key, UrlStore store) =>
{
    string? url = store.GetUrl(key);

    if (url == null)
    {
        return Results.NotFound(new { error = $"URL not found for key: {key}" });
    }

    return Results.Redirect(url);
})
.WithName("GetUrl");

app.Run();