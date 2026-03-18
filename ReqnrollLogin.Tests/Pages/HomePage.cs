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
        await _page.Locator($"text={linkText}").ClickAsync();
    }
}
