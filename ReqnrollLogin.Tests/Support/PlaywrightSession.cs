using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Support;

/// <summary>
/// Manages the full Playwright lifecycle: Playwright → Browser → Context → Page.
/// Implements IAsyncDisposable to ensure deterministic resource cleanup in reverse order.
/// </summary>
public class PlaywrightSession : IAsyncDisposable
{
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly IBrowserContext _context;
    private readonly IPage _page;
    private bool _disposed;

    private PlaywrightSession(IPlaywright playwright, IBrowser browser, IBrowserContext context, IPage page)
    {
        _playwright = playwright;
        _browser = browser;
        _context = context;
        _page = page;
    }

    public IPage Page => _page;

    /// <summary>
    /// Creates a new Playwright session with browser, context, and page.
    /// </summary>
    public static async Task<PlaywrightSession> CreateAsync(bool headless = false)
    {
        var playwright = await Playwright.CreateAsync();
        
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });
        
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });
        
        var page = await context.NewPageAsync();
        
        return new PlaywrightSession(playwright, browser, context, page);
    }

    /// <summary>
    /// Disposes resources in proper order: Page → Context → Browser → Playwright.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        try
        {
            // Cleanup in reverse order of creation
            if (_page != null)
            {
                await _page.CloseAsync();
            }

            if (_context != null)
            {
                await _context.CloseAsync();
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
            }

            // Playwright is a singleton-like object but should still be disposed
            if (_playwright != null)
            {
                _playwright.Dispose();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during Playwright cleanup: {ex}");
        }
        finally
        {
            _disposed = true;
        }
    }
}
