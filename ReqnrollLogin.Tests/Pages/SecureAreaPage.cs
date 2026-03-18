using Microsoft.Playwright;
// BUG: Missing System.Threading.Tasks namespace

namespace ReqnrollLogin.Tests.Pages;

public class SecureAreaPage
{
    private readonly IPage _page;

    public SecureAreaPage(IPage page)
    {
        _page = page;
    }

    public async Task<string> GetTitleAsync()
    {
        var title = _page.Locator("div.example h2");
        await title.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        var text = await title.TextContentAsync();
        return text?.Trim() ?? string.Empty;
    }

    public string GetPageUrl() => _page.Url;

    public async Task<string> GetSecureAreaMessageAsync()
    {
        var text = _page.Locator("div.example h4");
        await text.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        var content = await text.TextContentAsync();
        return content?.Trim() ?? string.Empty;
    }

    public async Task LogoutAsync()
    {
        await _page.Locator("a[href='/logout']").ClickAsync();
    }

    public async Task<string> GetFlashMessageAsync()
    {
        var flash = _page.Locator("#flash");
        await flash.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        var text = await flash.TextContentAsync();
        return text?.Trim() ?? string.Empty;
    }
}
