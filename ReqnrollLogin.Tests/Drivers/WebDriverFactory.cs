using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Drivers;

public static class BrowserFactory
{
    public static async Task<IPage> CreatePageAsync()
    {
        var playwright = await Playwright.CreateAsync();
        
        var headless = Environment.GetEnvironmentVariable("HEADLESS")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
        
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });
        
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });
        
        return await context.NewPageAsync();
    }
}
