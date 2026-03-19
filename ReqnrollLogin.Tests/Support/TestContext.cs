namespace ReqnrollLogin.Tests.Support;

/// <summary>
/// Shared test context that holds the Playwright session and provides access to the page.
/// </summary>
public class UiTestContext
{
    public PlaywrightSession? Session { get; set; }

    /// <summary>
    /// Convenience property to access the current page from the session.
    /// </summary>
    public Microsoft.Playwright.IPage? Page => Session?.Page;
}
