using Microsoft.Playwright;

namespace ReqnrollLogin.Tests.Pages;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    public async Task OpenAsync(string url)
    {
        await _page.GotoAsync(url);
    }

    public async Task NavigateByLinkTextAsync(string linkText)
    {
        // Use role-based selector for more stable and semantic matching
        // Exact: true ensures we match the exact link text, not partial matches
        var link = _page.GetByRole(AriaRole.Link, new() { NameString = linkText, Exact = true });
        await link.ClickAsync();
    }
}
