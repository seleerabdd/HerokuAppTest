namespace ReqnrollLogin.Tests.Drivers;

/// <summary>
/// Factory for creating Playwright sessions with proper resource ownership and lifecycle management.
/// </summary>
public static class BrowserFactory
{
    /// <summary>
    /// Creates a new Playwright session with all resources owned by the session object.
    /// The caller is responsible for disposing the session.
    /// </summary>
    public static async Task<ReqnrollLogin.Tests.Support.PlaywrightSession> CreateSessionAsync()
    {
        var headless = Environment.GetEnvironmentVariable("HEADLESS")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
        return await ReqnrollLogin.Tests.Support.PlaywrightSession.CreateAsync(headless: headless);
    }
}
